using AutoMapper;
using DeliveryStorage.Database.Entities;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.Domain.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BoxDb, Box>().ReverseMap();
        CreateMap<PalletDb, Pallet>().ReverseMap();
    }
}