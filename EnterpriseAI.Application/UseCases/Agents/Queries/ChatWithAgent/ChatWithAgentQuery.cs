using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Agents.Queries.ChatWithAgent;

public record ChatWithAgentQuery(Guid SessionId, string UserQuery) : IRequest<string>;
