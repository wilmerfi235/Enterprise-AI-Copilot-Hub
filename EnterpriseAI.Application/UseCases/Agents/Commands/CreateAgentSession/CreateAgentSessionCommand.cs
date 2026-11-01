using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Agents.Commands.CreateAgentSession;

public record CreateAgentSessionCommand(string UserId, string SessionTitle) : IRequest<Guid>;
