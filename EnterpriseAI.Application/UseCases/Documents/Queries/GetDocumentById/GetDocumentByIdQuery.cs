using EnterpriseAI.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Documents.Queries.GetDocumentById;

/// <summary>
/// 
/// Consulta
/// 
/// </summary>
/// <param name="Id"></param>
public record GetDocumentByIdQuery(Guid Id) : IRequest<DocumentDto?>;
