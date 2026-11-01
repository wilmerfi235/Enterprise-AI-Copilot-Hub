using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.Ports.AI;

/// <summary>
/// 
/// Define el puerto para interactuar con Modelos de Lenguaje (LLMs como OpenAI o Anthropic) para procesamiento de texto y 
/// generación de vectores (embeddings).
/// 
/// </summary>
public interface IAIService
{
    Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default);
    Task<float[]> GenerateEmbeddingsAsync(string text, CancellationToken cancellationToken = default);
}
