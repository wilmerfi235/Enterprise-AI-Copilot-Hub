using EnterpriseAI.Domain.Entities;
using EnterpriseAI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Documents.Commands.UploadDocument;

/// <summary>
/// 
/// </summary>
public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Guid>
{
    private readonly IDocumentRepository _documentRepository;

    public UploadDocumentCommandHandler(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<Guid> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        // 1. Crear entidad (aplica reglas de negocio del Dominio)
        var document = new Document(Guid.NewGuid(), request.Title, request.FilePath, request.FileSize);

        // 2. Guardar en repositorio
        await _documentRepository.AddAsync(document, cancellationToken);

        // 3. Retornar ID generado
        return document.Id;
    }
}
