using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Agents.Commands.CreateAgentSession;

public class CreateAgentSessionCommandHandler : IRequestHandler<CreateAgentSessionCommand, Guid>
{
    private readonly IAgentSessionRepository _sessionRepository;

    public CreateAgentSessionCommandHandler(IAgentSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Guid> Handle(CreateAgentSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new AgentSession(Guid.NewGuid(), request.UserId, request.SessionTitle);

        await _sessionRepository.AddAsync(session, cancellationToken);

        return session.Id;
    }
}
