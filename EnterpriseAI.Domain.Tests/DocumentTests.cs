using EnterpriseAI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.Domain.Tests;

public class DocumentTests
{
    [Fact]
    public void CreateDocument_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        string title = "Manual Técnico .NET 8";
        string filePath = "/storage/manual.pdf";
        long fileSize = 102450;

        // Act
        var document = new Document(id, title, filePath, fileSize);

        // Assert
        Assert.Equal(id, document.Id);
        Assert.Equal(title, document.Title);
        Assert.Equal(filePath, document.FilePath);
        Assert.Equal(fileSize, document.FileSize);
        Assert.False(document.IsProcessed);
        Assert.True(document.CreatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateDocument_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Document(Guid.NewGuid(), invalidTitle, "/storage/file.pdf", 500));
    }

    [Fact]
    public void MarkAsProcessed_ShouldChangeIsProcessedToTrue()
    {
        // Arrange
        var document = new Document(Guid.NewGuid(), "Guía Arquitectura", "/path/file.pdf", 2000);

        // Act
        document.MarkAsProcessed();

        // Assert
        Assert.True(document.IsProcessed);
    }
}
