using AutoMapper;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Database.Interfaces;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Services;

public class PalletService : IPalletService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<PalletDb> _palletRepository;

    public PalletService(IMapper mapper, IBaseRepository<PalletDb> palletRepository)
    {
        _mapper = mapper;
        _palletRepository = palletRepository;
    }
    
    public async Task<Pallet> GetByIdAsync(Guid id)
    {
        var finded = await _palletRepository.GetByIdAsync(pal => pal.Id == id, pal => pal.Boxes);
        return _mapper.Map<Pallet>(finded);
    }

    public async Task<Pallet> AddAsync(Pallet pallet)
    {
        var createDbModel = _mapper.Map<PalletDb>(pallet);
        var created = await _palletRepository.AddAsync(createDbModel);
        return _mapper.Map<Pallet>(created);
    }

    public async Task<Pallet> UpdateAsync(Pallet pallet)
    {
        var exists = await _palletRepository.ExistsAsync(pallet.Id);

        if (exists)
        {
            var updating = await _palletRepository.GetByIdAsync(pal => pal.Id == pallet.Id);
            updating.Height = pallet.Height;
            updating.Width = pallet.Width;
            updating.Depth = pallet.Depth;
            updating.Boxes = _mapper.Map<List<BoxDb>>(pallet.Boxes);
            
            var updated = await _palletRepository.UpdateAsync(updating);
            return _mapper.Map<Pallet>(updated);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _palletRepository.ExistsAsync(id);
        if (exists)
        {
            var deleting = await _palletRepository.GetByIdAsync(pal => pal.Id == id);
            await _palletRepository.DeleteAsync(deleting);
            return true;
        }
        return false;
    }

    public async Task<List<Pallet>> GetAllAsync()
    {
        var pallets = await _palletRepository.GetAllAsync(pallet => pallet.Boxes);
        return _mapper.Map<List<Pallet>>(pallets);
    }
}