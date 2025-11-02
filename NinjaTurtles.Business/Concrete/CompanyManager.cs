using AutoMapper;
using Microsoft.Extensions.Configuration;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Helpers.QrCodeGeneratator;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using System.Drawing.Imaging;
using static QRCoder.PayloadGenerator;

namespace NinjaTurtles.Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        private ICompanyDal _company;
        private ICompanyOrderDetailDal _companyOrderDetail;
        private IQrCodeMainDal _qrCodeMainDal;
        private IConfiguration _config;
        private IMapper _mapper;

        public CompanyManager(ICompanyDal company, ICompanyOrderDetailDal companyOrderDetail, IQrCodeMainDal qrCodeMainDal, IConfiguration config, IMapper mapper)
        {
            _company = company;
            _companyOrderDetail = companyOrderDetail;
            _qrCodeMainDal = qrCodeMainDal;
            _config = config;
            _mapper = mapper;
        }

        public IResult Add(AddCompanyDto dto)
        {
            var company = new Company();
            company.Name = dto.Name;
            company.ShortName = dto.ShortName;
            company.CreatedDate = DateTime.Now;
            company.IsActive = true;

            _company.Add(company);
            return new Result(true, Messages.CompanyAdded);
        }

        public IDataResult<List<CompanyOrderResponseDto>> GetList()
        {
            var result = _company.GetList(x => x.IsActive);
            var companyOrderDto = _mapper.Map<List<CompanyOrderResponseDto>>(result);

            return new SuccessDataResult<List<CompanyOrderResponseDto>>(companyOrderDto, Messages.CustomerGetList);
        }

        public async Task<IResult> AddDetail(AddCompanyOrderDetailDto dto)
        {
            try
            {


                var cod = new CompanyOrderDetail();
                cod.CompanyOrderId = dto.CompanyId;
                cod.ProductId = dto.ProductId;
                cod.Quantity = dto.Quantity;
                cod.LicenceUnitPrice = dto.LicenceUnitPrice;
                cod.CreatedDate = DateTime.Now;
                cod.IsActive = true;
                _companyOrderDetail.Add(cod);

                var company = _company.Get(c => c.Id == cod.CompanyOrderId);

                var companyOrderCount = _companyOrderDetail.GetList(c => c.CompanyOrderId == cod.CompanyOrderId && c.IsActive).Count();
                string directory = Path.Combine(@"D:\vhosts\karekodla.com\UploadFiles\QrCode",company.Name,cod.Id.ToString());
                DirectoryInfo di = Directory.CreateDirectory(directory);

                var url = "www.karekodla.com/Qr/";
                for (int i = 1; i <= cod.Quantity; i++)
                {
                    var guid = Guid.NewGuid();
                    var filePath = $"{directory}/{company.ShortName}#{companyOrderCount}#{i}.png";
                    var barcodeContent = url + guid;


                    var saved = await QRCodeHelper.GenerateQrWithLabelAsync(
    content: barcodeContent,
    labelText: $"{company.ShortName}#{companyOrderCount}#{i}",
    savePath: filePath,
    position: QRCodeHelper.LabelPosition.Bottom,
    pixelsPerModule: 20,
    fontFilePath: null // istersen TTF dosya yolu ver
);
                    var qrcodeMain = new QrCodeMain();
                    qrcodeMain.Id = guid;
                    qrcodeMain.CompanyOrderDetailId = cod.Id;
                    qrcodeMain.ImageUrl = filePath;
                    qrcodeMain.RedirectUrl = url;
                    qrcodeMain.CreatedDate = DateTime.Now;
                    qrcodeMain.IsActive = true;
                    _qrCodeMainDal.Add(qrcodeMain);
                }

                return new Result(true, Messages.CompanyOrderDetailAdded);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public IResult Update(UpdateCompanyDto dto)
        {
            var company = _company.Get(c => c.Id == dto.Id && c.IsActive);
            if (company == null)
                return new Result(false, Messages.DataNotFound);

            company.Name = dto.Name;
            company.ShortName = dto.ShortName;
            company.ModifiedDate = DateTime.Now;
            company.ModifiedBy = 1;
            _company.Update(company);
            return new Result(true, Messages.ProductUpdated);
        }
    }
}
