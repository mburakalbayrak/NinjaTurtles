using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface IQrService
    {
        Task<IDataResult<QrCodeDetailDto>> GetQrDetail(FilterQrDetailDto dto);
        IResult CreateHumanDetail(QrCodeHumanCreateDto dto);
        IResult CreateRedirectUrl(QrRedirectUrlDto dto);
        IResult UpdateRedirectUrl(QrRedirectUrlUpdateDto dto);
        IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto);
        IDataResult<QrCodeHumanDetailDto> GetHumanDetailVerify(QrUpdateVerifyDto dto);
        IResult UpdateHumanDetail(QrCodeHumanUpdateDto dto);
        IDataResult<QrCodeAnimalDetailDto> GetAnimalDetailVerify(QrUpdateVerifyDto dto);
        IResult UpdateAnimalDetail(QrCodeAnimalUpdateDto dto);
        Task SendQrReadMail(Guid id);

    }
}
