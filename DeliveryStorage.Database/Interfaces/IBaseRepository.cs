using System.Linq.Expressions;
using DeliveryStorage.Database.Entities;

namespace DeliveryStorage.Database.Interfaces;

public interface IBaseRepository<T>
{
    public Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task<bool> ExistsAsync(Guid id);
}