using EnterpriseAI.Application.UseCases.Documents.Commands.UploadDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAI.API.Controllers;

/// <summary>
/// 
/// Expone endpoints para la gestión y subida de documentos utilizando el patrón CQRS a través 
/// de MediatR.
/// 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<Guid>> UploadDocument([FromBody] UploadDocumentCommand command, CancellationToken cancellationToken)
    {
        var documentId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(UploadDocument), new { id = documentId }, documentId);
    }
}
