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

        // Configurar AgentSession y su colección dependiente SessionMessage
        modelBuilder.Entity<AgentSession>(entity =>
        {
            entity.HasKey(s => s.Id);

            // Indica a EF Core que SessionMessage es una colección poseída por AgentSession
            entity.OwnsMany(s => s.Messages, b =>
            {
                b.ToTable("AgentSessionMessages"); // Nombre de la tabla resultante en SQL Server
                b.WithOwner().HasForeignKey("AgentSessionId");
                b.Property<int>("Id"); // Clave primaria autoincremental para la tabla de mensajes
                b.HasKey("Id");
            });
        });

        // Aplica las configuraciones de Fluent API definidas en el ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
