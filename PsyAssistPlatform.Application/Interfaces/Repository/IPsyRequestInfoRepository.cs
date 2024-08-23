using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

namespace PsyAssistPlatform.Application.Interfaces.Repository;

public interface IPsyRequestInfoRepository
{
    Task<IEnumerable<IPsyRequestInfo>?> GetAllPsyRequestsAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestsByStatusIdAsync(int statusId, CancellationToken cancellationToken);
    
    Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestInfoByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IPsyRequestInfo?> GetPsyRequestByIdAsync(int id, CancellationToken cancellationToken);
}