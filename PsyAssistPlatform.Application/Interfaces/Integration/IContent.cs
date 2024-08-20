namespace PsyAssistPlatform.Application.Interfaces.Integration;

public interface IContent
{
    Task<string> GetContent(int psyId, int type, CancellationToken cancellationToken);
}
