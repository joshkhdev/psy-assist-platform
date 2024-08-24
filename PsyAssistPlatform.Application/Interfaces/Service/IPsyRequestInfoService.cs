using PsyAssistPlatform.Application.Interfaces.Dto.PsyRequest;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IPsyRequestInfoService
{
    Task<IEnumerable<IPsyRequestInfo>?> GetAllPsyRequestsInfoAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestsInfoByStatusIdAsync(int statusId, CancellationToken cancellationToken);

    Task<IEnumerable<IPsyRequestInfo>?> GetPsyRequestInfoByIdAsync(int id, CancellationToken cancellationToken);

    Task<IPsyRequestInfo?> ChangePsyRequestStatusAsync(IChangePsyRequestStatus changePsyRequestStatusData, CancellationToken cancellationToken);
    
    Task<IPsyRequestInfo?> RejectPsyRequestAsync(int psyRequestId, int userId, string comment, CancellationToken cancellationToken);
}