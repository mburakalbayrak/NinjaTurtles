using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using static QRCoder.PayloadGenerator;
using System.Drawing.Imaging;
using AutoMapper;
using NinjaTurtles.Core.Helpers.QrCodeGeneratator;

namespace NinjaTurtles.Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        private ICompanyDal _company;
        private ICompanyOrderDetailDal _companyOrderDetail;
        private IQrCodeMainDal _qrCodeMainDal;
        private IMapper _mapper;

        public CompanyManager(ICompanyDal companyOrder, ICompanyOrderDetailDal companyOrderDetail, IQrCodeMainDal qrCodeMainDal, IMapper mapper)
        {
            _company = companyOrder;
            _companyOrderDetail = companyOrderDetail;
            _qrCodeMainDal = qrCodeMainDal;
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

        public async Task<IResult> AddDetail(AddCompanyOrderDetailDto dto, string path)
        {
            try
            {
                var cod = new CompanyOrderDetail();
                cod.CompanyOrderId = dto.CompanyOrderId;
                cod.ProductId = dto.ProductId;
                cod.Quantity = dto.Quantity;
                cod.LicenceUnitPrice = dto.LicenceUnitPrice;
                cod.CreatedDate = DateTime.Now;
                cod.IsActive = true;
                _companyOrderDetail.Add(cod);

                var company = _company.Get(c => c.Id == cod.CompanyOrderId);

                var companyOrderCount = _companyOrderDetail.GetList(c => c.CompanyOrderId == cod.CompanyOrderId && c.IsActive).Count();
                var basedirectory = $"{path}/UploadFiles/QrCode/{company.Name}/{cod.Id}";
                DirectoryInfo di = Directory.CreateDirectory(basedirectory);

                var url = "www.karekodla.com/Qr/";
                for (int i = 1; i <= cod.Quantity; i++)
                {
                    var guid = Guid.NewGuid();
                    var filePath = $"{basedirectory}/{company.ShortName}#{companyOrderCount}#{i}.png";
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
    }
}
