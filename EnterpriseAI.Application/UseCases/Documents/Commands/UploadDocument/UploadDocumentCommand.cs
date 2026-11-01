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
/// <param name="Title"></param>
/// <param name="FilePath"></param>
/// <param name="FileSize"></param>
public record UploadDocumentCommand(string Title, string FilePath, long FileSize) : IRequest<Guid>;
