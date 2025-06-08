using DeliveryStorage.Database.Entities;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Interfaces;

public interface IAssignBoxService
{
    public Task<Pallet> AssignBoxToPallet(Guid palletId, List<Guid> boxIds);
}