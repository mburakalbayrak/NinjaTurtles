using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface IQrService
    {
        IDataResult<QrCodeDetailDto> GetQrDetail(Guid id);
        IResult CreateHumanDetail(QrCodeHumanCreateDto dto);
        IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto);
    }
}
