using EnterpriseAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Repositories;

/// <summary>
/// Contiene interfaces puras que definen cómo el dominio interactúa con el exterior (como bases de datos) para guardar o recuperar 
/// información. El dominio dice qué necesita, pero no cómo se implementa.
/// 
/// Define el contrato de persistencia para los documentos. La capa de infraestructura será la encargada de implementar esta 
/// interfaz usando EF Core y SQL Server.
/// 
/// </summary>
public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Document document, CancellationToken cancellationToken = default);
    Task UpdateAsync(Document document, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
