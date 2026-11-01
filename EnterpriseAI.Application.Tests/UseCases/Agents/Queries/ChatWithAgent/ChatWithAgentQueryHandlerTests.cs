using EnterpriseAI.Application.Ports.AI;
using EnterpriseAI.Application.Ports.VectorDb;
using EnterpriseAI.Application.UseCases.Agents.Queries.ChatWithAgent;
using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.Tests.UseCases.Agents.Queries.ChatWithAgent;

/// <summary>
/// 
/// En el constructor:
/// -----------------
/// Este manejador interactúa con tres subsistemas diferentes (repositorios, servicios de lenguaje y bases de datos vectoriales), 
/// inicializamos un Mock para cada puerto. 
/// Luego, se los inyectamos en conjunto al ChatWithAgentQueryHandler para simular un entorno completamente aislado.
/// 
/// Resultado: Valida que el orquestador se comunica de forma sincronizada con la IA y la base vectorial, 
/// garantizando que el usuario obtenga una respuesta fundamentada y que la conversación quede guardada 
/// en el historial.
/// 
/// </summary>
public class ChatWithAgentQueryHandlerTests
{
    private readonly Mock<IAgentSessionRepository> _sessionRepositoryMock;
    private readonly Mock<IAIService> _aiServiceMock;
    private readonly Mock<IVectorStoreService> _vectorStoreServiceMock;
    private readonly ChatWithAgentQueryHandler _handler;

    public ChatWithAgentQueryHandlerTests()
    {
        // Arrange general: Simulamos las dependencias externas (Repositorio, IA y Base de Datos Vectorial)
        _sessionRepositoryMock = new Mock<IAgentSessionRepository>();
        _aiServiceMock = new Mock<IAIService>();
        _vectorStoreServiceMock = new Mock<IVectorStoreService>();

        _handler = new ChatWithAgentQueryHandler(
            _sessionRepositoryMock.Object,
            _aiServiceMock.Object,
            _vectorStoreServiceMock.Object);
    }


    /// <summary>
    /// 
    /// Evalúa el camino feliz del flujo RAG (Retrieval-Augmented Generation): cuando la sesión existe y 
    /// la IA procesa la consulta exitosamente combinando contexto empresarial.
    /// 
    /// Valida que el orquestador se comunica de forma sincronizada con la IA y la base vectorial, garantizando 
    /// que el usuario obtenga una respuesta fundamentada y que la conversación quede guardada en el historial.
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WithValidSessionAndQuery_ShouldPerformRagAndReturnAiResponse()
    {
        // Arrange
        // Preparamos datos simulados de la sesión, la pregunta, los vectores numéricos (embeddings), 
        // los fragmentos de texto recuperados y la respuesta esperada del modelo de IA.
        var sessionId = Guid.NewGuid();
        var session = new AgentSession(sessionId, "user-123", "Sesión de Prueba");
        var query = new ChatWithAgentQuery(sessionId, "¿Cuál es la política de vacaciones de la empresa?");

        var dummyEmbedding = new float[] { 0.1f, 0.2f, 0.3f };
        var chunks = new List<string> { "Fragmento 1: La empresa otorga 15 días hábiles de vacaciones.", "Fragmento 2: Las vacaciones deben solicitarse con 15 días de anticipación." };
        var expectedAiResponse = "Basado en la política, tienes derecho a 15 días hábiles.";

        // Configuramos los 5 comportamientos esperados en cadena mediante Moq (.Setup)
        // 1. Simulamos que la sesión existe en el repositorio
        _sessionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        // 2. Simulamos que el servicio de IA genera correctamente el embedding de la pregunta
        _aiServiceMock
            .Setup(ai => ai.GenerateEmbeddingsAsync(query.UserQuery, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dummyEmbedding);

        // 3. Simulamos que la base de datos vectorial retorna fragmentos relevantes similares
        _vectorStoreServiceMock
            .Setup(vs => vs.SearchSimilarAsync(dummyEmbedding, 3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chunks);

        // 4. Simulamos que el LLM genera la respuesta enriquecida (RAG)
        _aiServiceMock
            .Setup(ai => ai.GenerateTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAiResponse);

        // 5. Simulamos el guardado de la actualización de la sesión
        _sessionRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<AgentSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedAiResponse, result);

        // Verificamos que se hayan agregado los mensajes (Pregunta del usuario y Respuesta de la IA) al agregado
        Assert.Equal(2, session.Messages.Count);

        // Verificamos que cada componente externo fue llamado exactamente una vez en su orden correcto
        _sessionRepositoryMock.Verify(repo => repo.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()), Times.Once);
        _aiServiceMock.Verify(ai => ai.GenerateEmbeddingsAsync(query.UserQuery, It.IsAny<CancellationToken>()), Times.Once);
        _vectorStoreServiceMock.Verify(vs => vs.SearchSimilarAsync(dummyEmbedding, 3, It.IsAny<CancellationToken>()), Times.Once);
        _aiServiceMock.Verify(ai => ai.GenerateTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _sessionRepositoryMock.Verify(repo => repo.UpdateAsync(session, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// 
    /// Evalúa el control de errores por recursos faltantes: qué ocurre si el usuario intenta 
    /// chatear en una sesión que no existe.
    /// 
    /// Asegura la robustez del sistema y previene consumo innecesario de tokens de IA o consultas a 
    /// bases de datos vectoriales cuando se envían identificadores de sesión inválidos o inexistentes.
    /// 
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WithNonExistingSessionId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var nonExistingSessionId = Guid.NewGuid();
        var query = new ChatWithAgentQuery(nonExistingSessionId, "Hola");

        // Configuramos el repositorio para que retorne null (sesión no encontrada)
        _sessionRepositoryMock
            .Setup(repo => repo.GetByIdAsync(nonExistingSessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AgentSession?)null);

        // Act & Assert
        // Verificamos que se lance la excepción KeyNotFoundException esperada
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));

        // Verificación de seguridad: Comprobamos que NINGÚN servicio de IA o base vectorial fue contactado
        _aiServiceMock.Verify(ai => ai.GenerateEmbeddingsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _vectorStoreServiceMock.Verify(vs => vs.SearchSimilarAsync(It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _aiServiceMock.Verify(ai => ai.GenerateTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _sessionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<AgentSession>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
