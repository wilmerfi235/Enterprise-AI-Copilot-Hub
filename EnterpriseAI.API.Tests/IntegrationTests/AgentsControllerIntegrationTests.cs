using EnterpriseAI.Application.UseCases.Agents.Commands.CreateAgentSession;
using EnterpriseAI.Application.UseCases.Agents.Queries.ChatWithAgent;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.API.Tests.IntegrationTests;

/// <summary>
/// 
/// Pruebas para el Controlador de Agentes
/// 
/// </summary>
public class AgentsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AgentsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateSession_WithValidPayload_ShouldReturnSessionId()
    {
        // Arrange
        var command = new CreateAgentSessionCommand(
            "user-test-123",
            "Sesión de Pruebas Automatizadas"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Agents/sessions", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var sessionId = await response.Content.ReadFromJsonAsync<Guid>();
        sessionId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ChatWithAgent_WithNonExistingSession_ShouldReturnBadRequestOrError()
    {
        // Arrange
        var query = new ChatWithAgentQuery(
            Guid.NewGuid(), // ID de sesión inexistente
            "¿Cómo funciona el sistema?"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Agents/chat", query);

        // Assert
        // Esperamos un error controlado (por ejemplo, BadRequest o NotFound manejado por el middleware)
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }
}
