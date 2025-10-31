using AutoMapper;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AddCustomerDto, Customer>().ReverseMap();
            CreateMap<QrCodeHumanDetail, QrCodeHumanDetailDto>().ReverseMap();
            CreateMap<QrCodeAnimalDetail, QrCodeAnimalDetailDto>().ReverseMap();
            CreateMap<QrCodeAnimalCreateDto, QrCodeAnimalDetail>().ReverseMap();
            CreateMap<QrCodeAnimalUpdateDto, QrCodeAnimalDetail>().ReverseMap();
            CreateMap<QrCodeHumanUpdateDto, QrCodeHumanDetail>().ReverseMap();
            CreateMap<CompanyOrderResponseDto, Company>().ReverseMap();


        }
    }
}
