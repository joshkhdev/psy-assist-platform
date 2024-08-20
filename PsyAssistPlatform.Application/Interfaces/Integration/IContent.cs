namespace PsyAssistPlatform.Application.Interfaces.Integration;

public interface IContentService
{
    Task<string> GetContent(int psyId, int type, CancellationToken cancellationToken);
}
