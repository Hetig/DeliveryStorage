using AutoMapper;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Database.Interfaces;
using DeliveryStorage.Domain.Interfaces;
using DeliveryStorage.Domain.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace DeliveryStorage.Domain.Services;

public class AssignBoxService : IAssignBoxService
{
    private readonly IBaseRepository<BoxDb> _boxRepository;
    private readonly IBaseRepository<PalletDb> _palletRepository;
    private readonly IMapper _mapper;

    public AssignBoxService(IMapper mapper, IBaseRepository<BoxDb> boxRepository, IBaseRepository<PalletDb> palletRepository)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
        _mapper = mapper;
    }

    public async Task<Pallet> AssignBoxToPallet(Guid palletId, List<Guid> boxIds)
    {
        var existingPallet = await _palletRepository.GetByIdAsync(pal => pal.Id == palletId, pal => pal.Boxes);
        if (existingPallet == null) return null;
        
        var boxes = new List<BoxDb>();
        foreach (var boxId in boxIds)
        {
            var box = await _boxRepository.GetByIdAsync(box => box.Id == boxId);
            if (box == null || existingPallet.Width + existingPallet.Height < box.Width + box.Height)
            {
                boxes.Clear();
                return null;
            }
            boxes.Add(box);
        }

        existingPallet.Boxes = boxes;
        return _mapper.Map<Pallet>(await _palletRepository.UpdateAsync(existingPallet));
    }
}