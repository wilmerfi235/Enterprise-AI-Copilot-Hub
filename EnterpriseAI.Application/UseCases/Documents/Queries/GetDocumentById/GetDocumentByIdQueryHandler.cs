using EnterpriseAI.Application.DTOs;
using EnterpriseAI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Documents.Queries.GetDocumentById;

/// <summary>
/// 
/// Manejador
/// 
/// </summary>
public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto?>
{
    private readonly IDocumentRepository _documentRepository;

    public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<DocumentDto?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (document == null) return null;

        return new DocumentDto(
            document.Id,
            document.Title,
            document.FilePath,
            document.FileSize,
            document.CreatedAt,
            document.IsProcessed
        );
    }
}
