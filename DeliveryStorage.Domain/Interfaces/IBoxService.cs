using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Interfaces;

public interface IBoxService
{
    public Task<Box> GetByIdAsync(Guid id);
    public Task<Box> AddAsync(Box box);
    public Task<Box> UpdateAsync(Box box);
    public Task<bool> DeleteAsync(Guid id);
    public Task<List<Box>> GetAllAsync();
}