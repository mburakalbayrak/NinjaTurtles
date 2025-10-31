using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Entities;
using NinjaTurtles.Core.Entities.Enums;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Concrete
{
    public class QrManager : IQrService
    {
        private IQrCodeMainDal _qrCodeMainDal;
        private IQrCodeHumanDetailDal _qrCodeHumanDetailDal;
        private IQrCodeAnimalDetailDal _qrCodeAnimalDetailDal;
        private ICustomerDal _customerDal;
        private ICustomerQrVerificationDal _customerQrVerificationDal;
        private IParamItemDal _paramItemDal;
        private IMapper _mapper;

        public QrManager(IQrCodeMainDal qrCodeMainDal, IQrCodeHumanDetailDal qrCodeHumanDetailDal, IQrCodeAnimalDetailDal qrCodeAnimalDetailDal, ICustomerDal customerDal, ICustomerQrVerificationDal customerQrVerificationDal, IParamItemDal paramItemDal, IMapper mapper)
        {
            _qrCodeMainDal = qrCodeMainDal;
            _qrCodeHumanDetailDal = qrCodeHumanDetailDal;
            _qrCodeAnimalDetailDal = qrCodeAnimalDetailDal;
            _customerDal = customerDal;
            _customerQrVerificationDal = customerQrVerificationDal;
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
                return new Result(false, message: Messages.DataNotFound);
            try
            {
                var qrHuman = _mapper.Map<QrCodeHumanDetail>(dto);

                qr.DetailTypeId = 1;
                qr.CustomerId = dto.CustomerId;
                _qrCodeHumanDetailDal.Add(qrHuman);
                _qrCodeMainDal.Update(qr);
                return new Result(true, Messages.QrFilled);
            }
            catch (Exception ex)
            {
                return new Result(true, ex.Message);
            }
        }

        public IDataResult<QrCodeAnimalDetailDto> GetAnimalDetailVerify(QrUpdateVerifyDto dto)
        {
            var date = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > date);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(message: Messages.VerifyCodeExpired);

            var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == dto.QrMainId);
            if (qrAnimal == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);
            var animalDto = _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal);
            return new SuccessDataResult<QrCodeAnimalDetailDto>(data: animalDto, message: Messages.AccountVerifySuccess);
        }

        public IDataResult<QrCodeHumanDetailDto> GetHumanDetailVerify(QrUpdateVerifyDto dto)
        {
            var date = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > date);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(message: Messages.VerifyCodeExpired);

            var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == dto.QrMainId);
            if (qrHuman == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(null, Messages.DataNotFound);
            var paramList = _paramItemDal.GetList();

            var qrDto = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);

            qrDto.Gender = qrHuman.GenderId != null
               ? paramList.FirstOrDefault(c => c.Id == qrHuman.GenderId)?.Name : null;

            qrDto.MaritalStatus = qrHuman.MaritalStatusId != null
                 ? paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name : null;

            qrDto.EducationStatus = qrHuman.EducationStatusId != null
? paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name : null;

            qrDto.CityOfResidence = qrHuman.CityOfResidenceId != null
                ? paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name : null;

            qrDto.BloodType = qrHuman.BloodTypeId != null
                ? paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name : null;

            qrDto.Profession = qrHuman.ProfessionId != null
                ? paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name : null;

            qrDto.PrimaryRelation = qrHuman.PrimaryRelationId != null
                ? paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name : null;

            qrDto.SecondaryRelation = qrHuman.SecondaryRelationId != null
                ? paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name : null; return new SuccessDataResult<QrCodeHumanDetailDto>(data: qrDto, message: Messages.AccountVerifySuccess);
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
                         ? paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name : null;

                    qrDto.HumanDetail.EducationStatus = qrHuman.EducationStatusId != null
    ? paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name : null;

                    qrDto.HumanDetail.CityOfResidence = qrHuman.CityOfResidenceId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name : null;

                    qrDto.HumanDetail.BloodType = qrHuman.BloodTypeId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name : null;

                    qrDto.HumanDetail.Profession = qrHuman.ProfessionId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name : null;

                    qrDto.HumanDetail.PrimaryRelation = qrHuman.PrimaryRelationId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name : null;

                    qrDto.HumanDetail.SecondaryRelation = qrHuman.SecondaryRelationId != null
                        ? paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name : null;
                    break;
                case 2:
                    var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
                    qrDto.AnimalDetail = _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal);
                    break;
            }

            return new SuccessDataResult<QrCodeDetailDto>(qrDto);
        }

        public IResult UpdateAnimalDetail(QrCodeAnimalUpdateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeAnimalUpdateDto>(data: null, message: Messages.DataNotFound);

            qr.DetailTypeId = 2;
            _qrCodeMainDal.Update(qr);
            var qrAnimal = _mapper.Map<QrCodeAnimalDetail>(dto);
            _qrCodeAnimalDetailDal.Update(qrAnimal);
            return new Result(true, Messages.QrFilled);
            throw new NotImplementedException();
        }

        public IResult UpdateHumanDetail(QrCodeHumanUpdateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeHumanUpdateDto>(data: null, message: Messages.DataNotFound);

            qr.DetailTypeId = 1;
            _qrCodeMainDal.Update(qr);
            var qrHuman = _mapper.Map<QrCodeHumanDetail>(dto);
            _qrCodeHumanDetailDal.Update(qrHuman);
            return new Result(true, Messages.QrFilled);

        }
    }
}
