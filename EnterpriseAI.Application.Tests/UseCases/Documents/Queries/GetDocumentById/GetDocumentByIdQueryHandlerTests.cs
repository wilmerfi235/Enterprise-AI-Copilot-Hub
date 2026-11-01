using EnterpriseAI.Application.UseCases.Documents.Queries.GetDocumentById;
using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EnterpriseAI.Application.Tests.UseCases.Documents.Queries.GetDocumentById;

/// <summary>
/// 
/// En el constructor:
/// -----------------
/// Nos permite probar el manejador de consultas (GetDocumentByIdQueryHandler) sin necesidad de levantar 
/// una base de datos real, inyectando el mock directamente en su constructor.
/// 
/// </summary>
public class GetDocumentByIdQueryHandlerTests
{
    private readonly Mock<IDocumentRepository> _documentRepositoryMock;
    private readonly GetDocumentByIdQueryHandler _handler;

    public GetDocumentByIdQueryHandlerTests()
    {
        // Arrange general: Simulamos el repositorio del dominio
        _documentRepositoryMock = new Mock<IDocumentRepository>();
        _handler = new GetDocumentByIdQueryHandler(_documentRepositoryMock.Object);
    }

    /// <summary>
    /// 
    /// Evalúa el camino feliz (Happy Path): cuando el cliente solicita un documento cuyo ID sí existe en el sistema.
    /// 
    /// Valida que la consulta transforma correctamente una Entidad de Dominio en un Objeto de 
    /// Transferencia de Datos (DocumentDto) conservando íntegramente la información.
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WithExistingId_ShouldReturnDocumentDto()
    {
        // 1. ARRANGE (Organizar)
        var documentId = Guid.NewGuid();
        var existingDocument = new Document(documentId, "Clean Architecture en .NET", "/docs/architecture.pdf", 5000);

        // Le decimos a Moq: "Cuando el repositorio reciba un GetByIdAsync con este ID exacto, 
        // no busques en ninguna base de datos; simplemente retorna este objeto 'existingDocument'".
        _documentRepositoryMock
            .Setup(repo => repo.GetByIdAsync(documentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDocument);

        var query = new GetDocumentByIdQuery(documentId);

        // 2. ACT (Actuar)
        // Ejecutamos la consulta pasándola por el manejador (Handler)
        var result = await _handler.Handle(query, CancellationToken.None);

        // 3. ASSERT (Comprobar)
        // Verificamos que el resultado no sea nulo y que el DTO devuelto 
        // contenga exactamente las mismas propiedades que tenía la entidad original del dominio.
        Assert.NotNull(result);
        Assert.Equal(documentId, result.Id);
        Assert.Equal(existingDocument.Title, result.Title);
        Assert.Equal(existingDocument.FilePath, result.FilePath);
        Assert.Equal(existingDocument.FileSize, result.FileSize);
        Assert.Equal(existingDocument.IsProcessed, result.IsProcessed);
    }

    /// <summary>
    /// 
    /// Evalúa el escenario alternativo de ausencia de datos: qué ocurre si se intenta consultar un ID 
    /// que no existe en la base de datos.
    /// 
    /// Asegura que el sistema es resiliente ante peticiones de recursos inexistentes y no explota con 
    /// excepciones no controladas, devolviendo un resultado nulo seguro
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WithNonExistingId_ShouldReturnNull()
    {
        // 1. ARRANGE (Organizar)
        var nonExistingId = Guid.NewGuid();

        // Configuramos el mock para que retorne null explícitamente ante un ID desconocido
        _documentRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Document?)null);

        var query = new GetDocumentByIdQuery(nonExistingId);

        // 2. ACT (Actuar)
        var result = await _handler.Handle(query, CancellationToken.None);

        // 3. ASSERT (Comprobar)
        // Comprobamos que el manejador maneja la situación devolviendo un valor nulo (null),
        // lo cual le permitirá a la capa API responder con un código HTTP 404 (NotFound) de manera limpia.
        Assert.Null(result);
    }
}
