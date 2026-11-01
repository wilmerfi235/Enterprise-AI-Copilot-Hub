using EnterpriseAI.Application.UseCases.Agents.Commands.CreateAgentSession;
using EnterpriseAI.Application.UseCases.Agents.Queries.ChatWithAgent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAI.API.Controllers;

/// <summary>
/// 
/// Expone endpoints para la creación de sesiones de agentes y la interacción directa mediante 
/// consultas RAG.
/// 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AgentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AgentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sessions")]
    public async Task<ActionResult<Guid>> CreateSession([FromBody] CreateAgentSessionCommand command, CancellationToken cancellationToken)
    {
        var sessionId = await _mediator.Send(command, cancellationToken);
        return Ok(sessionId);
    }

    [HttpPost("chat")]
    public async Task<ActionResult<string>> Chat([FromBody] ChatWithAgentQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(new { response });
    }
}
