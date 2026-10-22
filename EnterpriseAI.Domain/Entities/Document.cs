using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Entities;

/// <summary>
/// 
/// Representa un documento corporativo cargado en la plataforma para la posterior vectorización 
/// y recuperación de información (RAG).
/// 
/// Modela un documento del negocio. Encapsula las reglas para asegurar que no se cree un documento 
/// sin título, ruta o tamaño válido, protegiendo su integridad.
/// 
/// </summary>
public class Document
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string FilePath { get; private set; }
    public long FileSize { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsProcessed { get; private set; }

    // Constructor protegido o privado para garantizar la validación DDD
    private Document() { }

    public Document(Guid id, string title, string filePath, long fileSize)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("El título del documento es obligatorio.") : title;
        FilePath = string.IsNullOrWhiteSpace(filePath) ? throw new ArgumentException("La ruta del archivo es obligatoria.") : filePath;
        FileSize = fileSize > 0 ? fileSize : throw new ArgumentException("El tamaño del archivo debe ser mayor a cero.");
        CreatedAt = DateTime.UtcNow;
        IsProcessed = false;
    }

    // Comportamiento de negocio dentro de la entidad
    public void MarkAsProcessed()
    {
        IsProcessed = true;
    }
}
