using AutoMapper;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Database.Interfaces;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Services;

public class BoxService : IBoxService
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<BoxDb> _boxRepository;

    public BoxService(IMapper mapper, IBaseRepository<BoxDb> boxRepository)
    {
        _mapper = mapper;
        _boxRepository = boxRepository;
    }
    
    public async Task<Box> GetByIdAsync(Guid id)
    {
        var finded = await _boxRepository.GetByIdAsync(id);
        return _mapper.Map<Box>(finded);
    }

    public async Task<Box> AddAsync(Box box)
    {
        var createDbModel = _mapper.Map<BoxDb>(box);
        var created = await _boxRepository.AddAsync(createDbModel);
        return _mapper.Map<Box>(created);
    }

    public async Task<Box> UpdateAsync(Box box)
    {
        var exists = await _boxRepository.ExistsAsync(box.Id);

        if (exists)
        {
            var updating = await _boxRepository.GetByIdAsync(box.Id);
            updating.Height = box.Height;
            updating.Width = box.Width;
            updating.Weight = box.Weight;
            updating.ProductionDate = box.ProductionDate;
            
            await _boxRepository.UpdateAsync(updating);
            return _mapper.Map<Box>(updating);
        }
        
        return null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _boxRepository.ExistsAsync(id);
        if (exists)
        {
            var deleting = await _boxRepository.GetByIdAsync(id);
            await _boxRepository.DeleteAsync(deleting);
            return true;
        }
        return false;
    }

    public async Task<List<Box>> GetAllAsync()
    {
        var boxes = _mapper.Map<List<Box>>(await _boxRepository.GetAllAsync());
        return boxes;
    }
}