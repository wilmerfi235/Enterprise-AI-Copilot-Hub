using EnterpriseAI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Tests;

/// <summary>
/// 
/// Crea un archivo llamado DocumentChunkIdTests.cs para validar la lógica de creación de identificadores de fragmentos vectoriales.
/// 
/// </summary>
public class DocumentChunkIdTests
{
    [Fact]
    public void Unique_ShouldCreateNonEmptyGuid()
    {
        // Act
        var chunkId = DocumentChunkId.Unique();

        // Assert
        Assert.NotNull(chunkId);
        Assert.NotEqual(Guid.Empty, chunkId.Value);
    }

    [Fact]
    public void From_WithValidGuid_ShouldReturnDocumentChunkId()
    {
        // Arrange
        var validGuid = Guid.NewGuid();

        // Act
        var chunkId = DocumentChunkId.From(validGuid);

        // Assert
        Assert.Equal(validGuid, chunkId.Value);
    }

    [Fact]
    public void From_WithEmptyGuid_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => DocumentChunkId.From(Guid.Empty));
    }
}
