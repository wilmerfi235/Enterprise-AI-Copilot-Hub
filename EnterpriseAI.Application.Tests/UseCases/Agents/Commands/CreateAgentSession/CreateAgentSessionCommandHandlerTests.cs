using EnterpriseAI.Application.UseCases.Agents.Commands.CreateAgentSession;
using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.Tests.UseCases.Agents.Commands.CreateAgentSession;

public class CreateAgentSessionCommandHandlerTests
{
    private readonly Mock<IAgentSessionRepository> _sessionRepositoryMock;
    private readonly CreateAgentSessionCommandHandler _handler;

    public CreateAgentSessionCommandHandlerTests()
    {
        // Arrange general: Simulamos el repositorio de sesiones del dominio usando Moq
        _sessionRepositoryMock = new Mock<IAgentSessionRepository>();
        _handler = new CreateAgentSessionCommandHandler(_sessionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateSessionAndReturnGuid()
    {
        // Arrange
        var command = new CreateAgentSessionCommand("user-123", "Asistente Financiero IA");

        _sessionRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<AgentSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        _sessionRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.Is<AgentSession>(s => s.UserId == command.UserId &&
                                         s.SessionTitle == command.SessionTitle),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Handle_WithInvalidUserId_ShouldThrowArgumentException(string invalidUserId)
    {
        // Arrange
        var command = new CreateAgentSessionCommand(invalidUserId, "Asistente Financiero");

        // Act & Assert
        // Verificamos que el Dominio intercepte el UserId inválido y lance la excepción esperada
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, CancellationToken.None));

        // Verificamos por seguridad que NUNCA se intentó guardar nada en el repositorio
        _sessionRepositoryMock.Verify(
            repo => repo.AddAsync(It.IsAny<AgentSession>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Handle_WithNullOrEmptyTitle_ShouldAssignDefaultTitleAndSucceed(string invalidTitle)
    {
        // Arrange
        var command = new CreateAgentSessionCommand("user-123", invalidTitle);

        _sessionRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<AgentSession>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        // Verificamos que el dominio aplicó correctamente el título por defecto "Nueva Sesión"
        _sessionRepositoryMock.Verify(
            repo => repo.AddAsync(
                It.Is<AgentSession>(s => s.UserId == "user-123" && s.SessionTitle == "Nueva Sesión"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
