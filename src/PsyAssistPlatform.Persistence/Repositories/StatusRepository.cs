using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class StatusRepository : IRepository<Status>
{
    private readonly DbSet<Status> _dbSet;
    private readonly IMemoryCache _memoryCache;
    private const string StatusCacheName = "Status_{0}";

    public StatusRepository(PsyAssistContext context, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _dbSet = context.Set<Status>();
    }
    
    public async Task<IEnumerable<Status>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(StatusCacheName, "All");
        var statuses = await _memoryCache.GetOrCreateAsync(cacheKey,
            async _ => await _dbSet.AsNoTracking().ToListAsync(cancellationToken));

        return statuses!;
    }

    public async Task<IEnumerable<Status>> GetAsync(Expression<Func<Status, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Status?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(StatusCacheName, id);
        var status = await _memoryCache.GetOrCreateAsync(cacheKey,
            async _ => await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken));

        return status;
    }

    public Task<Status> AddAsync(Status entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Status> UpdateAsync(Status entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}