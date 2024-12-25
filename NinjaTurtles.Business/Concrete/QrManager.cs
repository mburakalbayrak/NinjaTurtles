using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Entities.Enums;
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
        private IParamItemDal _paramItemDal;
        private IMapper _mapper;

        public QrManager(IQrCodeMainDal qrCodeMainDal, IQrCodeHumanDetailDal qrCodeHumanDetailDal, IQrCodeAnimalDetailDal qrCodeAnimalDetailDal, IParamItemDal paramItemDal, IMapper mapper)
        {
            _qrCodeMainDal = qrCodeMainDal;
            _qrCodeHumanDetailDal = qrCodeHumanDetailDal;
            _qrCodeAnimalDetailDal = qrCodeAnimalDetailDal;
            _paramItemDal = paramItemDal;
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
            var qr = _qrCodeMainDal.Get(c => c.Id == id && c.IsActive && !c.IsDeleted);
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(null, Messages.DataNotFound);

            var qrDto = new QrCodeDetailDto
            {
                DetailTypeId = qr.DetailTypeId ?? default,
                Empty = !qr.DetailTypeId.HasValue
            };

            if (qrDto.Empty)
                return new SuccessDataResult<QrCodeDetailDto>(qrDto, Messages.EmptyQr);

            var paramList = _paramItemDal.GetList();

            switch (qr.DetailTypeId)
            {
                case 1:
                    var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == qr.Id);
                    qrDto.HumanDetail = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);

                    qrDto.HumanDetail.Gender = qrHuman.GenderId != null
                       ? paramList.FirstOrDefault(c => c.Id == qrHuman.GenderId)?.Name : null;

                   qrDto.HumanDetail.MaritalStatus = qrHuman.MaritalStatusId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name: null;

                    qrDto.HumanDetail.EducationStatus = qrHuman.EducationStatusId != null
    ? paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name: null;

                    qrDto.HumanDetail.CityOfResidence = qrHuman.CityOfResidenceId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name: null;

                    qrDto.HumanDetail.BloodType = qrHuman.BloodTypeId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name: null;

                    qrDto.HumanDetail.Profession = qrHuman.ProfessionId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name: null;

                    qrDto.HumanDetail.PrimaryRelation = qrHuman.PrimaryRelationId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name: null;

                    qrDto.HumanDetail.SecondaryRelation = qrHuman.SecondaryRelationId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name: null;
                    break;
                case 2:
                    var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
                    qrDto.AnimalDetail = _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal);
                    break;
            }

            return new SuccessDataResult<QrCodeDetailDto>(qrDto);


        }
    }
}
