using DeliveryStorage.Database.Entities;

namespace DeliveryStorage.Database.Interfaces;

public interface IBaseRepository<T>
{
    public Task<T> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task<bool> ExistsAsync(Guid id);
}