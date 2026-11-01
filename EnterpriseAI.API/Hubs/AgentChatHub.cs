using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace EnterpriseAI.API.Hubs;

/// <summary>
/// 
/// Permite el streaming de respuestas de los agentes de IA hacia los clientes conectados
/// 
/// </summary>
public class AgentChatHub : Hub
{
    public async Task SendMessageToGroup(string sessionId, string message)
    {
        // Lógica para enviar mensajes en tiempo real a una sesión específica
        await Clients.Group(sessionId).SendAsync("ReceiveMessage", "Agent", message);
    }

    public async Task JoinSessionGroup(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    }
}
