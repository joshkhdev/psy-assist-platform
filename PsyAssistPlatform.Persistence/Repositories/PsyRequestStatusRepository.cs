using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class PsyRequestStatusRepository : IPsyRequestStatusRepository
{
    private readonly PsyAssistContext _context;
    private readonly DbSet<PsyRequestStatus> _dbSet;
    
    public PsyRequestStatusRepository(PsyAssistContext context)
    {
        _context = context;
        _dbSet = context.Set<PsyRequestStatus>();
    }
    
    public async Task<IEnumerable<PsyRequestStatus>?> GetAllStatusesAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PsyRequestStatus>?> GetAllStatusesByPsyRequestIdAsync(int psyRequestId, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking()
            .Where(psyRequestStatus => psyRequestStatus.PsyRequestId == psyRequestId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PsyRequestStatus> GetLastStatusByPsyRequestIdAsync(int psyRequestId, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking()
            .Where(psyRequestStatus => psyRequestStatus.PsyRequestId == psyRequestId)
            .OrderByDescending(psyRequestStatus => psyRequestStatus.StatusUpdateDate)
            .FirstAsync(cancellationToken);
    }

    public async Task<PsyRequestStatus> AddPsyRequestStatusAsync(
        PsyRequestStatus psyRequestStatus,
        CancellationToken cancellationToken)
    {
        _dbSet.Add(psyRequestStatus);
        await _context.SaveChangesAsync(cancellationToken);

        return psyRequestStatus;
    }
}