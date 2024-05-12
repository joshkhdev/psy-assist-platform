using PsyAssistPlatform.Application.Interfaces.Dto.Status;

namespace PsyAssistPlatform.Application.Interfaces.Service;

public interface IStatusService
{
    Task<IEnumerable<IStatus>> GetStatusesAsync(CancellationToken cancellationToken);
    
    Task<IStatus?> GetStatusByIdAsync(int id, CancellationToken cancellationToken);
}