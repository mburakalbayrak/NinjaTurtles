using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface IQrService
    {
        IDataResult<QrCodeDetailDto> GetQrDetail(Guid id);
        IResult CreateHumanDetail(QrCodeHumanCreateDto dto);
        IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto);

        IDataResult<QrCodeHumanDetailDto> GetHumanDetailVerify(QrUpdateVerifyDto dto);
        IResult UpdateHumanDetail(QrCodeHumanUpdateDto dto);
        IDataResult<QrCodeAnimalDetailDto> GetAnimalDetailVerify(QrUpdateVerifyDto dto);

        IResult UpdateAnimalDetail(QrCodeAnimalUpdateDto dto);
    }
}
