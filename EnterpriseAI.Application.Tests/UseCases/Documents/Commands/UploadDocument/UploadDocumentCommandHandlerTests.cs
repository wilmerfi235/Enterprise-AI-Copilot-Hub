using EnterpriseAI.Application.UseCases.Documents.Commands.UploadDocument;
using EnterpriseAI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using EnterpriseAI.Domain.Entities;

namespace EnterpriseAI.Application.Tests.UseCases.Documents.Commands.UploadDocument;

/// <summary>
/// 
/// En el constructor:
/// -----------------
/// Como estamos probando la capa de aplicación de forma aislada, no queremos conectarnos a una
/// base de datos real. Por ello, utilizamos la librería Moq para crear un Mock (un doble o simulador) 
/// del repositorio de documentos (IDocumentRepository).
/// 
/// Luego, inyectamos ese repositorio simulado dentro del manejador (UploadDocumentCommandHandler), 
/// dejándolos listos para ser probados.
/// 
/// 
/// </summary>
public class UploadDocumentCommandHandlerTests
{
    private readonly Mock<IDocumentRepository> _documentRepositoryMock;
    private readonly UploadDocumentCommandHandler _handler;

    public UploadDocumentCommandHandlerTests()
    {
        // Arrange general: Simulamos el repositorio del dominio usando Moq
        _documentRepositoryMock = new Mock<IDocumentRepository>();
        _handler = new UploadDocumentCommandHandler(_documentRepositoryMock.Object);
    }

    /// <summary>
    /// 
    /// Este método evalúa el camino feliz (Happy Path): cuando el usuario envía información correcta 
    /// para registrar un documento.
    /// 
    /// Resultado de la prueba: Valida que el flujo completo de subida funciona, que se genera un identificador 
    /// único de tipo Guid como respuesta y que la orden de guardado sí llegó al repositorio de la base de datos.
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateDocumentAndReturnGuid()
    {
        // Arrange (Preparar)
        var command = new UploadDocumentCommand("Manual Técnico .NET 8", "/storage/manual.pdf", 102450);

        // Configuramos el mock para que, cuando se llame a AddAsync con cualquier documento, 
        // devuelva una tarea completada con éxito (simulando que guardó en la BD).
        _documentRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        // Ejecutamos el método Handle del manejador pasándole el comando.
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Comprobamos que el resultado no sea un Guid vacío (es decir, que devolvió un ID válido).
        Assert.NotEqual(Guid.Empty, result);

        // Verificamos mediante el Mock que el repositorio fue llamado EXACTAMENTE una vez (Times.Once)
        // y que los datos del documento coinciden con los que envió el comando.
        _documentRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.Is<Document>(d => d.Title == command.Title &&
                                     d.FilePath == command.FilePath &&
                                     d.FileSize == command.FileSize),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// 
    /// Este método evalúa el camino de error o validación: qué sucede si se intenta procesar un comando 
    /// con datos incorrectos (en este caso, títulos vacíos, nulos o con espacios).
    /// 
    /// [Theory] permite ejecutar la misma prueba varias veces automáticamente inyectando los distintos 
    /// valores definidos en las etiquetas [InlineData]
    /// 
    /// </summary>
    /// <param name="invalidTitle"></param>
    /// <param name="filePath"></param>
    /// <param name="fileSize"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("", "/storage/manual.pdf", 102450)]
    [InlineData("   ", "/storage/manual.pdf", 102450)]
    [InlineData(null, "/storage/manual.pdf", 102450)]
    public async Task Handle_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle, string filePath, long fileSize)
    {
        // Arrange
        // Creamos el comando utilizando los datos inválidos provistos por los atributos [InlineData].
        var command = new UploadDocumentCommand(invalidTitle, filePath, fileSize);

        // Act & Assert
        // El dominio valida los parámetros y lanza una excepción que intercepta el flujo
        // Verificamos que al ejecutar el manejador se lance obligatoriamente una excepción del tipo ArgumentException
        // (provocada por las reglas de validación del Dominio).
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        // Verificamos que NUNCA (Times.Never) se intentó guardar nada en el repositorio, 
        // protegiendo la base de datos de almacenar datos corruptos o inválidos.
        _documentRepositoryMock.Verify(
            repo => repo.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
