using EnterpriseAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Tests;

/// <summary>
/// 
/// Crea un nuevo archivo llamado AgentSessionTests.cs en el proyecto de pruebas para verificar que las sesiones 
/// de chat de los agentes autónomos validen correctamente sus datos y permitan registrar mensajes de forma segura.
/// 
/// </summary>
public class AgentSessionTests
{
    [Fact]
    public void CreateAgentSession_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        string userId = "user-123";
        string title = "Consulta de Arquitectura RAG";

        // Act
        var session = new AgentSession(id, userId, title);

        // Assert
        Assert.Equal(id, session.Id);
        Assert.Equal(userId, session.UserId);
        Assert.Equal(title, session.SessionTitle);
        Assert.True(session.StartedAt <= DateTime.UtcNow);
        Assert.Empty(session.Messages); // La sesión inicia sin mensajes
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateAgentSession_WithInvalidUserId_ShouldThrowArgumentException(string invalidUserId)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new AgentSession(Guid.NewGuid(), invalidUserId, "Sesión de Prueba"));
    }

    [Fact]
    public void AddMessage_WithValidContent_ShouldAddMessageToCollection()
    {
        // Arrange
        var session = new AgentSession(Guid.NewGuid(), "user-123", "Prueba de Chat");
        string sender = "User";
        string content = "¿Cómo configuro el patrón CQRS en .NET 8?";

        // Act
        session.AddMessage(sender, content);

        // Assert
        var message = Assert.Single(session.Messages);
        Assert.Equal(sender, message.Sender);
        Assert.Equal(content, message.Content);
        Assert.True(message.Timestamp <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void AddMessage_WithInvalidContent_ShouldThrowArgumentException(string invalidContent)
    {
        // Arrange
        var session = new AgentSession(Guid.NewGuid(), "user-123", "Prueba de Chat");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            session.AddMessage("Agent", invalidContent));
    }
}
