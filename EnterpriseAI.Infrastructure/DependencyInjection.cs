using EnterpriseAI.Application.Ports.AI;
using EnterpriseAI.Application.Ports.VectorDb;
using EnterpriseAI.Domain.Repositories;
using EnterpriseAI.Infrastructure.ExternalServices.AI;
using EnterpriseAI.Infrastructure.Persistencia.Relational;
using EnterpriseAI.Infrastructure.Persistencia.Vector;
using EnterpriseAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configurar el contexto de EF Core con SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // 2. Registrar Repositorios (Puertos del Dominio)
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IAgentSessionRepository, AgentSessionRepository>();

        // 3. Registrar Servicios Externos de IA (HTTP Client para OpenAIService)
        services.AddHttpClient<IAIService, OpenAIService>();

        // 4. Registrar Base de Datos Vectorial (Qdrant)
        services.AddSingleton<IVectorStoreService, QdrantVectorStoreService>();

        return services;
    }
}
