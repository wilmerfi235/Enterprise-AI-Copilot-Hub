using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.Ports.VectorDb;

/// <summary>
/// 
/// Define el contrato para almacenar y buscar fragmentos de documentos (chunks) mediante búsqueda semántica en 
/// bases de datos vectoriales (ej. Qdrant).
/// 
/// </summary>
public interface IVectorStoreService
{
    Task UpsertChunkAsync(Guid documentId, Guid chunkId, string content, float[] vector, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> SearchSimilarAsync(float[] queryVector, int topK = 3, CancellationToken cancellationToken = default);
}
