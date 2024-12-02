using AutoMapper;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<QrCodeHumanDetail, QrCodeHumanDetailDto>();
            CreateMap<QrCodeAnimalDetail, QrCodeAnimalDetailDto>();
            CreateMap<QrCodeHumanCreateDto, QrCodeHumanDetail>();
        }
    }
}
