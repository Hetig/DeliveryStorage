using DeliveryStorage.Database.Data;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryStorage.Database.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DatabaseContext _databaseContext;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = _databaseContext.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        var created = await _dbSet.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
        return created.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            var updated = _dbSet.Update(entity);
            await _databaseContext.SaveChangesAsync();
            return updated.Entity;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}