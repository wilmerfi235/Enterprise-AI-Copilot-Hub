using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using EnterpriseAI.Infrastructure.Persistencia.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Infrastructure.Repositories;

/// <summary>
/// 
/// ULTIMA CLASE IMPLEMENTADA, CONTINUAR DESDE ESTE PUNTO.
/// 
/// </summary>
public class AgentSessionRepository : IAgentSessionRepository
{
    private readonly ApplicationDbContext _context;

    public AgentSessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AgentSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AgentSessions.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(AgentSession session, CancellationToken cancellationToken = default)
    {
        await _context.AgentSessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AgentSession session, CancellationToken cancellationToken = default)
    {
        _context.AgentSessions.Update(session);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
