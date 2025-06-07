using AutoMapper;
using DeliveryStorage.API.Dtos;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BoxDto, Box>().ReverseMap();
        CreateMap<CreateBoxDto, Box>().ReverseMap();
        
        CreateMap<PalletDto, Pallet>().ReverseMap();
        CreateMap<CreatePalletDto, Pallet>().ReverseMap();
    }
}