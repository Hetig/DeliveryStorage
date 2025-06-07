using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Interfaces;

public interface IPalletService
{
    public Task<Pallet> GetByIdAsync(Guid id);
    public Task<Pallet> AddAsync(Pallet pallet);
    public Task<Pallet> UpdateAsync(Pallet pallet);
    public Task<bool> DeleteAsync(Guid id);
    public Task<List<Pallet>> GetAllAsync();
}