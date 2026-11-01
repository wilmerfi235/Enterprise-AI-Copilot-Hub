using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Entities;

/// <summary>
/// 
/// Modela una sesión de chat o interacción entre un usuario y los Agentes de Inteligencia Artificial del sistema.
/// 
/// Administra el historial de conversaciones de los agentes de IA de forma encapsulada, permitiendo registrar mensajes 
/// de manera estructurada bajo un patrón de agregado DDD.
/// 
/// </summary>
public class AgentSession
{
    public Guid Id { get;  private set; }
    public string UserId { get; private set; }
    public string SessionTitle { get; private set; }
    public DateTime StartedAt { get; private set; }

    private readonly List<SessionMessage> _messages = new();
    public IReadOnlyCollection<SessionMessage> Messages => _messages.AsReadOnly();

    public AgentSession(Guid id, string userId, string sessionTitle)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        UserId = string.IsNullOrWhiteSpace(userId) ? throw new ArgumentException("El ID de usuario es obligatorio.") : userId;
        SessionTitle = string.IsNullOrWhiteSpace(sessionTitle) ? "Nueva Sesión" : sessionTitle;
        StartedAt = DateTime.UtcNow;
    }

    public void AddMessage(string sender, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("El contenido del mensaje no puede estar vacío.");

        _messages.Add(new SessionMessage(sender, content, DateTime.UtcNow));
    }
}

// 
/// <summary>
/// 
/// Objeto interno o sub-entidad del agregado
/// Dado que SessionMessage actúa como una colección dependiente dentro de AgentSession, debes indicarle a 
/// Entity Framework Core mediante Fluent API cómo debe tratar esta relación (como un tipo poseído u Owned Type).
/// 
/// </summary>
public class SessionMessage
{
    public string Sender { get; private set; }
    public string Content { get; private set; }
    public DateTime Timestamp { get; private set; }

    // Constructor privado sin parámetros requerido por EF Core
    private SessionMessage() { }

    internal SessionMessage(string sender, string content, DateTime timestamp)
    {
        Sender = sender;
        Content = content;
        Timestamp = timestamp;
    }
}
