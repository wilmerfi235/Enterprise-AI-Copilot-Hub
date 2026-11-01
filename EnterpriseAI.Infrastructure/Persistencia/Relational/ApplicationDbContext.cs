using EnterpriseAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Infrastructure.Persistencia.Relational;

/// <summary>
/// 
/// Configura el contexto de Entity Framework Core para conectarse a SQL Server y mapear 
/// las entidades del dominio.
/// 
/// Función: Actúa como la sesión de trabajo con la base de datos SQL Server mediante EF Core.
/// 
/// </summary>
public class ApplicationDbContext : DbContext
{
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<AgentSession> AgentSessions => Set<AgentSession>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica las configuraciones de Fluent API definidas en el ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
