using System.Linq.Expressions;
using PsyAssistPlatform.Domain;

namespace PsyAssistPlatform.Application.Interfaces;

public interface IRepository<T>
    where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task AddAsync(T entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}