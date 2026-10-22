using EnterpriseAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Repositories;

/// <summary>
/// 
/// Contrato para la persistencia de las sesiones de chat de los agentes autónomos.
/// 
/// </summary>
public interface IAgentSessionRepository
{
    Task<AgentSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(AgentSession session, CancellationToken cancellationToken = default);
    Task UpdateAsync(AgentSession session, CancellationToken cancellationToken = default);
}
