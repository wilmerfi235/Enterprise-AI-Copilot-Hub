using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.API.Tests.IntegrationTests;

public class AgentChatHubIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AgentChatHubIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Client_Can_Connect_And_Join_Session_Group()
    {
        // Arrange
        var client = _factory.CreateClient();
        var serverUrl = _factory.Server.BaseAddress.ToString();

        // Construir la conexión de SignalR apuntando al servidor de pruebas en memoria
        var connection = new HubConnectionBuilder()
            .WithUrl($"{serverUrl}hubs/agentChat", o =>
            {
                o.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
            })
            .Build();

        // Act
        await connection.StartAsync();

        // Probar la invocación de un método del Hub
        Func<Task> act = async () => await connection.InvokeAsync("JoinSessionGroup", "test-session-123");

        // Assert
        connection.State.Should().Be(HubConnectionState.Connected);
        await act.Should().NotThrowAsync();

        await connection.StopAsync();
    }
}
