using AutoMapper;
using DeliveryStorage.API.Dtos;
using DeliveryStorage.Domain.Models;

namespace DeliveryStorage.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateBoxQueryDto, Box>().ReverseMap();
        CreateMap<CreateBoxQueryDto, Box>().ReverseMap();
        CreateMap<GetBoxResponseDto, Box>().ReverseMap();
        
        CreateMap<UpdatePalletQueryDto, Pallet>().ReverseMap();
        CreateMap<CreatePalletQueryDto, Pallet>().ReverseMap();
        CreateMap<GetPalletResponseDto, Pallet>().ReverseMap();
        CreateMap<UpdatePalletResponseDto, Pallet>().ReverseMap();
    }
}