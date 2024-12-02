using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using static QRCoder.PayloadGenerator;
using System.Drawing.Imaging;
using System.IO;
using NinjaTurtles.Core.Helpers.QrCode;
using NinjaTurtles.Core.Entities;
using AutoMapper;

namespace NinjaTurtles.Business.Concrete
{
    public class CompanyOrderManager : ICompanyOrderService
    {
        private ICompanyOrderDal _companyOrder;
        private ICompanyOrderDetailDal _companyOrderDetail;
        private IQrCodeMainDal _qrCodeMainDal;
        private IMapper _mapper;

        public CompanyOrderManager(ICompanyOrderDal companyOrder, ICompanyOrderDetailDal companyOrderDetail, IQrCodeMainDal qrCodeMainDal, IMapper mapper)
        {
            _companyOrder = companyOrder;
            _companyOrderDetail = companyOrderDetail;
            _qrCodeMainDal = qrCodeMainDal;
            _mapper = mapper;
        }

        public IResult Add(string name)
        {
            var company = new CompanyOrder();
            company.Name = name;
            company.CreatedDate = DateTime.Now;
            company.CreatedBy = 1;
            company.IsActive = true;

            _companyOrder.Add(company);
            return new Result(true, Messages.CustomerAdded);
        }

        public IDataResult<List<CompanyOrderResponseDto>> GetList()
        {
            var result = _companyOrder.GetList(x=> x.IsActive);
            var companyOrderDto = _mapper.Map<List<CompanyOrderResponseDto>>(result);

            return new SuccessDataResult<List<CompanyOrderResponseDto>>(companyOrderDto, Messages.CustomerGetList);
        }

        public IResult AddDetail(AddCompanyOrderDetailDto dto,string path)
        {
            try
            {
                var cod = new CompanyOrderDetail();
                cod.CompanyOrderId = dto.CompanyOrderId;
                cod.ProductId = dto.ProductId;
                cod.Quantity = dto.Quantity;
                cod.LicenceUnitPrice = dto.LicenceUnitPrice;
                cod.CreatedDate = DateTime.Now;
                cod.CreatedBy = 1;
                cod.IsActive = true;
                _companyOrderDetail.Add(cod);

                var company = _companyOrder.Get(c => c.Id == cod.CompanyOrderId);


                var basedirectory = $"{path}/UploadFiles/QrCode/{company.Name}/{cod.Id}";
                DirectoryInfo di = Directory.CreateDirectory(basedirectory);

                var url = "www.karekodla.com/Qr/";
                for (int i = 1; i <= cod.Quantity; i++)
                {
                    var guid = Guid.NewGuid();
                    var filePath = $"{basedirectory}/{guid}.png";
                    var barcodeContent = url + guid;
                    var result = QRCodeHelper.SaveQRCodeAsImage(barcodeContent, filePath, ImageFormat.Png);

                    var qrcodeMain = new QrCodeMain();
                    qrcodeMain.Id = guid;
                    qrcodeMain.ImageUrl = filePath;
                    qrcodeMain.RedirectUrl = url;
                    qrcodeMain.CreatedDate = DateTime.Now;
                    qrcodeMain.CreatedBy = 1;
                    qrcodeMain.IsActive = true;
                    _qrCodeMainDal.Add(qrcodeMain);
                }

                return new Result(true, Messages.CompanyOrderDetailAdded);
            }
            catch (Exception ex)
            {
                return new Result(false,ex.Message);
            }
        }
    }
}
