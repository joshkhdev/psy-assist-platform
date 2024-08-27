using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PsyAssistPlatform.Application.Interfaces.Repository;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class PsyRequestRepository : IRepository<PsyRequest>
{
    private readonly PsyAssistContext _context;
    private readonly DbSet<PsyRequest> _dbSet;
    private readonly IMemoryCache _memoryCache;
    private const string PsyRequestCacheName = "PsyRequest_{0}";
    
    public PsyRequestRepository(PsyAssistContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _dbSet = context.Set<PsyRequest>();
    }
    
    public async Task<IEnumerable<PsyRequest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PsyRequest>> GetAsync(Expression<Func<PsyRequest, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<PsyRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(PsyRequestCacheName, id);
        var psyRequest = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(1));

            return await _dbSet.AsNoTracking()
                .Include(psyRequest => psyRequest.PsychologistProfile)
                .Include(psyRequest => psyRequest.Questionnaire)
                .ThenInclude(questionnaire => questionnaire.Contact)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        });

        return psyRequest;
    }

    public async Task<PsyRequest> AddAsync(PsyRequest entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<PsyRequest> UpdateAsync(PsyRequest entity, CancellationToken cancellationToken)
    {
        _memoryCache.Remove(string.Format(PsyRequestCacheName, entity.Id));
        
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity;
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}