using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PsyAssistPlatform.Application.Interfaces;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Persistence.Repositories;

public class EfCoreRepository<T> : IRepository<T>
    where T : BaseEntity
{
    private readonly PsyAssistContext _context;
    private readonly DbSet<T> _dbSet;
    
    public EfCoreRepository(PsyAssistContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.FindAsync<T>(id, cancellationToken);
        if (entity is not null)
            _dbSet.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}