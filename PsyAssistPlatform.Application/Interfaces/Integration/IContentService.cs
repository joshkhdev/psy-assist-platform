namespace PsyAssistPlatform.Application.Interfaces.Integration;

public interface IContentService
{
    Task<string> GetContentAsync(int psyId, int type, CancellationToken cancellationToken);
}
