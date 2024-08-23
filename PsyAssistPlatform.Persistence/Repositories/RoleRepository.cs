using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class RoleRepository : IRepository<Role>
{
    private readonly DbSet<Role> _dbSet;
    private readonly IMemoryCache _memoryCache;
    private const string RoleCacheName = "Role_{0}";
    
    public RoleRepository(PsyAssistContext context, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _dbSet = context.Set<Role>();
    }
    
    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(RoleCacheName, "All");
        var roles = await _memoryCache.GetOrCreateAsync(cacheKey,
            async _ => await _dbSet.AsNoTracking().ToListAsync(cancellationToken));

        return roles!;
    }

    public async Task<IEnumerable<Role>> GetAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(RoleCacheName, id);
        var role = await _memoryCache.GetOrCreateAsync(cacheKey,
            async _ => await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken));

        return role;
    }

    public Task<Role> AddAsync(Role entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Role> UpdateAsync(Role entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}