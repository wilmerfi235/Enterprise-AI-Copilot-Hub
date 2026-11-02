using EnterpriseAI.Application.UseCases.Documents.Commands.UploadDocument;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseAI.API.Tests.IntegrationTests;

/// <summary>
/// 
/// Pruebas para el Controlador de Documentos
/// 
/// </summary>
public class DocumentsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DocumentsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UploadDocument_WithValidPayload_ShouldReturnCreatedStatusAndId()
    {
        // Arrange
        var command = new UploadDocumentCommand(
            "Manual de Clean Architecture",
            "/docs/architecture.pdf",
            102450
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/Documents/upload", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var documentId = await response.Content.ReadFromJsonAsync<Guid>();
        documentId.Should().NotBeEmpty();
    }
}
