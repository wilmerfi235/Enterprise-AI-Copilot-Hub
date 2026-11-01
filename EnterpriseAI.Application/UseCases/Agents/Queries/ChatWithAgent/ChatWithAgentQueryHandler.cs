using EnterpriseAI.Application.Ports.AI;
using EnterpriseAI.Application.Ports.VectorDb;
using EnterpriseAI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Application.UseCases.Agents.Queries.ChatWithAgent;

public class ChatWithAgentQueryHandler : IRequestHandler<ChatWithAgentQuery, string>
{
    private readonly IAgentSessionRepository _sessionRepository;
    private readonly IAIService _aiService;
    private readonly IVectorStoreService _vectorStoreService;

    public ChatWithAgentQueryHandler(
        IAgentSessionRepository sessionRepository,
        IAIService aiService,
        IVectorStoreService vectorStoreService)
    {
        _sessionRepository = sessionRepository;
        _aiService = aiService;
        _vectorStoreService = vectorStoreService;
    }

    public async Task<string> Handle(ChatWithAgentQuery request, CancellationToken cancellationToken)
    {
        // 1. Obtener la sesión de chat
        var session = await _sessionRepository.GetByIdAsync(request.SessionId, cancellationToken)
                      ?? throw new KeyNotFoundException("La sesión de agente especificada no existe.");

        // 2. Registrar el mensaje del usuario en el agregado
        session.AddMessage("User", request.UserQuery);

        // 3. Generar vector (embedding) de la consulta para RAG
        var queryEmbedding = await _aiService.GenerateEmbeddingsAsync(request.UserQuery, cancellationToken);

        // 4. Buscar contexto relevante en la base de datos vectorial
        var relevantChunks = await _vectorStoreService.SearchSimilarAsync(queryEmbedding, topK: 3, cancellationToken);
        var context = string.Join("\n---\n", relevantChunks);

        // 5. Construir prompt enriquecido para el LLM
        var augmentedPrompt = $"Contexto empresarial:\n{context}\n\nPregunta del usuario: {request.UserQuery}";

        // 6. Obtener respuesta del modelo de IA
        var aiResponse = await _aiService.GenerateTextAsync(augmentedPrompt, cancellationToken);

        // 7. Registrar respuesta del Agente en la sesión
        session.AddMessage("Agent", aiResponse);
        await _sessionRepository.UpdateAsync(session, cancellationToken);

        return aiResponse;
    }
}
