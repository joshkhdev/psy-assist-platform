using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Interfaces.Repository;

public interface IPsyRequestStatusRepository
{
    Task<IEnumerable<PsyRequestStatus>?> GetAllStatusesAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<PsyRequestStatus>?> GetAllStatusesByPsyRequestIdAsync(int psyRequestId, CancellationToken cancellationToken);
    
    Task<PsyRequestStatus> GetLastStatusByPsyRequestIdAsync(int psyRequestId, CancellationToken cancellationToken);

    Task<PsyRequestStatus> AddPsyRequestStatusAsync(PsyRequestStatus psyRequestStatus, CancellationToken cancellationToken);
}