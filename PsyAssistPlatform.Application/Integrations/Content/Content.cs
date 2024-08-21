using System.Net.Http.Json;
using PsyAssistPlatform.Application.Interfaces.Integration;

namespace PsyAssistPlatform.Application.Integrations.Service;

public class ContentService : IContentService
{
    private readonly HttpClient _httpClient;

    public ContentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetContent(int psyId, int type, CancellationToken cancellationToken)
    {
        var fileMetadata = new FileMetadata() { PsyId = psyId.ToString(), Type = type };
        var response = await _httpClient.PostAsJsonAsync(string.Empty, fileMetadata, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
