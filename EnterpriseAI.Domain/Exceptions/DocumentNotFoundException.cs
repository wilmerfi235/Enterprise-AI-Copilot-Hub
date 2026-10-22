using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Exceptions;

/// <summary>
/// 
/// Excepción específica lanzada cuando se intenta buscar, procesar o eliminar un documento que no existe, permitiendo un manejo 
/// de errores limpio y expresivo en los casos de uso.
/// 
/// </summary>
public class DocumentNotFoundException : DomainException
{
    public DocumentNotFoundException(Guid documentId)
        : base($"El documento con el ID '{documentId}' no fue encontrado en el sistema.")
    {
    }
}
