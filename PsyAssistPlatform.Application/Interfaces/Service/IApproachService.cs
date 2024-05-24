using PsyAssistPlatform.Application.Interfaces.Dto.Approach;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IApproachService
{
    Task<IEnumerable<IApproach>> GetApproachesAsync(CancellationToken cancellationToken);

    Task<IApproach?> GetApproachByIdAsync(int id, CancellationToken cancellationToken);

    Task<IApproach> CreateApproachAsync(ICreateApproach approachData, CancellationToken cancellationToken);
    
    Task<IApproach> UpdateApproachAsync(int id, IUpdateApproach approachData, CancellationToken cancellationToken);
    
    Task DeleteApproachAsync(int id, CancellationToken cancellationToken);
}