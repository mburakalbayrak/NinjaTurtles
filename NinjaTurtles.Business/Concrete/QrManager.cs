using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Concrete
{
    public class QrManager : IQrService
    {
        private IQrCodeMainDal _qrCodeMainDal;
        private IQrCodeHumanDetailDal _qrCodeHumanDetailDal;
        private IQrCodeAnimalDetailDal _qrCodeAnimalDetailDal;
        private IMapper _mapper;

        public QrManager(IQrCodeMainDal qrCodeMainDal, IQrCodeHumanDetailDal qrCodeHumanDetailDal, IQrCodeAnimalDetailDal qrCodeAnimalDetailDal, IMapper mapper)
        {
            _qrCodeMainDal = qrCodeMainDal;
            _qrCodeHumanDetailDal = qrCodeHumanDetailDal;
            _qrCodeAnimalDetailDal = qrCodeAnimalDetailDal;
            _mapper = mapper;
        }
        public IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(data: null, message: Messages.DataNotFound);

            var qrAnimal = _mapper.Map<QrCodeAnimalDetail>(dto);

            if (qr.DetailTypeId == null)
            {
                qr.DetailTypeId = 2;
                qr.CustomerId = dto.CustomerId;
                _qrCodeAnimalDetailDal.Add(qrAnimal);
                _qrCodeMainDal.Update(qr);
                return new Result(true, Messages.QrFilled);
            }

            if (qr.DetailTypeId == 2)
            {
                _qrCodeAnimalDetailDal.Update(qrAnimal);
                return new Result(true, Messages.QrFilled);
            }

            return new Result(false, Messages.DataNotFound);
        }
        public IResult CreateHumanDetail(QrCodeHumanCreateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(data: null, message: Messages.DataNotFound);

            var qrHuman = _mapper.Map<QrCodeHumanDetail>(dto);

            if (qr.DetailTypeId == null)
            {
                qr.DetailTypeId = 1;
                qr.CustomerId = dto.CustomerId;
                _qrCodeHumanDetailDal.Add(qrHuman);
                _qrCodeMainDal.Update(qr);
                return new Result(true, Messages.QrFilled);
            }

            if (qr.DetailTypeId == 1)
            {
                _qrCodeHumanDetailDal.Update(qrHuman);
                return new Result(true, Messages.QrFilled);
            }

            return new Result(false, Messages.DataNotFound);
        }

        public IDataResult<QrCodeDetailDto> GetQrDetail(Guid id)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == id && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(data: null, message: Messages.DataNotFound);

            var qrDto = new QrCodeDetailDto
            {
                DetailTypeId = qr.DetailTypeId ?? default,
                Empty = !qr.DetailTypeId.HasValue
            };

            if (qr.DetailTypeId == 1)
            {
                var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == qr.Id);
                qrDto.HumanDetail = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);
            }
            else if (qr.DetailTypeId == 2)
            {
                var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
                qrDto.AnimalDetail = _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal);
            }

            var message = qr.DetailTypeId.HasValue ? null : Messages.EmptyQr;
            return new SuccessDataResult<QrCodeDetailDto>(qrDto, message);
        }
    }
}
