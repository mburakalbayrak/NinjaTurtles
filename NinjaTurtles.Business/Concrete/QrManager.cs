using AutoMapper;
using Microsoft.Extensions.Configuration;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.DataAccess.Concrete.Dto;
using NinjaTurtles.Core.Helpers.FileUpload;
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
        private ICustomerQrVerificationDal _customerQrVerificationDal;
        private IParamItemDal _paramItemDal;
        private IConfiguration _config;
        private IMapper _mapper;

        public QrManager(IQrCodeMainDal qrCodeMainDal, IQrCodeHumanDetailDal qrCodeHumanDetailDal, IQrCodeAnimalDetailDal qrCodeAnimalDetailDal, ICustomerQrVerificationDal customerQrVerificationDal, IParamItemDal paramItemDal, IConfiguration config, IMapper mapper)
        {
            _qrCodeMainDal = qrCodeMainDal;
            _qrCodeHumanDetailDal = qrCodeHumanDetailDal;
            _qrCodeAnimalDetailDal = qrCodeAnimalDetailDal;
            _customerQrVerificationDal = customerQrVerificationDal;
            _paramItemDal = paramItemDal;
            _config = config;
            _mapper = mapper;
        }

        public IResult CreateAnimalDetail(QrCodeAnimalCreateDto dto)
        {
            var qr = _qrCodeMainDal.Get(c => c.Id == dto.QrMainId && (!c.IsDeleted && c.IsActive));
            if (qr == null)
                return new ErrorDataResult<QrCodeDetailDto>(data: null, message: Messages.DataNotFound);

            var qrAnimal = _mapper.Map<QrCodeAnimalDetail>(dto);

            qr.DetailTypeId = 2;
            qr.CustomerId = dto.CustomerId;
            _qrCodeAnimalDetailDal.Add(qrAnimal);
            _qrCodeMainDal.Update(qr);
            return new Result(true, Messages.QrFilled);

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
                if (dto.File != null)
                {
                    //var currentdirectory = @"D:\vhosts\karekodla.com\UploadFiles\ProfilePictures\";
                    string directory = _config.GetSection("Directories:FileDirectory").Value;

                    string path = System.IO.Path.Combine(directory, "ProfilePictures");
                    CreateFileWithFileNameDto uploadFile = new CreateFileWithFileNameDto()
                    {
                        File = dto.File,
                        FolderPath = path,
                        FileName = dto.File.FileName
                    };
                    var file = WriteFile.CreateFileWithFileName(uploadFile).Data;
                    qrHuman.ProfilePictureUrl = dto.File.Name;
                }
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
            var now = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > now);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(message: Messages.VerifyCodeExpired);

            var qr = _qrCodeMainDal.Get(c => c.CustomerId == hasCode.CustomerId && c.Id == dto.QrMainId);
            if (qr == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);

            var qrAnimal = _qrCodeAnimalDetailDal.Get(c => c.QrMainId == qr.Id);
            if (qrAnimal == null)
                return new ErrorDataResult<QrCodeAnimalDetailDto>(null, Messages.DataNotFound);

            return new SuccessDataResult<QrCodeAnimalDetailDto>(
                _mapper.Map<QrCodeAnimalDetailDto>(qrAnimal),
                Messages.AccountVerifySuccess
            );
        }

        public IDataResult<QrCodeHumanDetailDto> GetHumanDetailVerify(QrUpdateVerifyDto dto)
        {
            var now = DateTime.Now;
            var hasCode = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > now);
            if (hasCode == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(message: Messages.VerifyCodeExpired);

            var qr = _qrCodeMainDal.Get(c => c.CustomerId == hasCode.CustomerId && c.Id == dto.QrMainId);
            if (qr == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(null, Messages.DataNotFound);

            var qrHuman = _qrCodeHumanDetailDal.Get(c => c.QrMainId == qr.Id);
            if (qrHuman == null)
                return new ErrorDataResult<QrCodeHumanDetailDto>(null, Messages.DataNotFound);

            var paramList = _paramItemDal.GetList();
            var qrDto = _mapper.Map<QrCodeHumanDetailDto>(qrHuman);

            qrDto.Gender = paramList.FirstOrDefault(c => c.Id == qrHuman.GenderId)?.Name;
            qrDto.MaritalStatus = paramList.FirstOrDefault(c => c.Id == qrHuman.MaritalStatusId)?.Name;
            qrDto.EducationStatus = paramList.FirstOrDefault(c => c.Id == qrHuman.EducationStatusId)?.Name;
            qrDto.CityOfResidence = paramList.FirstOrDefault(c => c.Id == qrHuman.CityOfResidenceId)?.Name;
            qrDto.BloodType = paramList.FirstOrDefault(c => c.Id == qrHuman.BloodTypeId)?.Name;
            qrDto.Profession = paramList.FirstOrDefault(c => c.Id == qrHuman.ProfessionId)?.Name;
            qrDto.PrimaryRelation = paramList.FirstOrDefault(c => c.Id == qrHuman.PrimaryRelationId)?.Name;
            qrDto.SecondaryRelation = paramList.FirstOrDefault(c => c.Id == qrHuman.SecondaryRelationId)?.Name;

            return new SuccessDataResult<QrCodeHumanDetailDto>(qrDto, Messages.AccountVerifySuccess);
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

                    string directory = _config.GetSection("Directories:FileDirectory").Value;

                    string filePath = Path.Combine(directory, "ProfilePictures", qrHuman.ProfilePictureUrl);
                    FileInfo fileInfo = new FileInfo(filePath);
                    var bytes = File.ReadAllBytes(filePath);

                    qrDto.HumanDetail.ProfilePictureData = Convert.ToBase64String(bytes);

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

            if (dto.File != null)
            {
                string directory = _config.GetSection("Directories:FileDirectory").Value;

                string path = System.IO.Path.Combine(_config.GetSection("Directories:FileRootUpload").Value, "ProfilePictures");
                CreateFileWithFileNameDto uploadFile = new CreateFileWithFileNameDto()
                {
                    File = dto.File,
                    FolderPath = path,
                    FileName = dto.File.FileName
                };
                var file = WriteFile.CreateFileWithFileName(uploadFile).Data;
                var oldFile = System.IO.Path.Combine(_config.GetSection("Directories:FileRootUpload").Value, "ProfilePictures", qrHuman.ProfilePictureUrl);
                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);

                qrHuman.ProfilePictureUrl = dto.File.Name;
            }
            _qrCodeHumanDetailDal.Update(qrHuman);
            return new Result(true, Messages.QrFilled);

        }
    }
}
