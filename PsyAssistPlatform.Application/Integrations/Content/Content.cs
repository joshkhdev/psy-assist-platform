using System.Net.Http.Json;
using PsyAssistPlatform.Application.Interfaces.Integration;

namespace PsyAssistPlatform.Application.Integrations.Content;

public class Content : IContent
{
    private readonly HttpClient _httpClient;

    public Content(HttpClient httpClient)
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
