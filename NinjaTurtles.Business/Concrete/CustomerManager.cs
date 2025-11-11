using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.DataAccess.Concrete.Dto;
using NinjaTurtles.Core.Entities.Enums;
using NinjaTurtles.Core.Helpers;
using NinjaTurtles.Core.Helpers.FileUpload;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using System.Net;

namespace NinjaTurtles.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;
        private ICustomerQrVerificationDal _customerQrVerificationDal;
        private IParamItemDal _paramItemDal;
        private ICustomerContractDal _customerContractDal;
        private IHttpContextAccessor _httpContextAccessor;
        private IConverter _converter;
        private IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, ICustomerQrVerificationDal customerQrVerificationDal, IParamItemDal paramItemDal, ICustomerContractDal customerContractDal, IHttpContextAccessor httpContextAccessor, IConverter converter, IMapper mapper)
        {
            _customerDal = customerDal;
            _customerQrVerificationDal = customerQrVerificationDal;
            _paramItemDal = paramItemDal;
            _customerContractDal = customerContractDal;
            _httpContextAccessor = httpContextAccessor;
            _converter = converter;
            _mapper = mapper;
        }

        public async Task<IResult> Add(AddCustomerDto dto)
        {
            try
            {
                var returnMessage = Messages.HasCustomer;
                var hasCustomer = _customerDal.GetList(c => (c.Email == dto.Email || c.PhoneNumber == dto.PhoneNumber) && !c.IsDeleted && c.IsActive).LastOrDefault();
                if (hasCustomer == null)
                {
                    hasCustomer = _mapper.Map<Customer>(dto);
                    hasCustomer.IsDeleted = false;
                    hasCustomer.IsActive = false;
                    hasCustomer.CreatedDate = DateTime.Now;
                    _customerDal.Add(hasCustomer);
                    returnMessage = Messages.CustomerAdded;
                }

                var code = ProExt.NextSixDigitCode();
                MailWorker mail = new MailWorker();

                mail.Init(StaticVars.VerifyMailUserName, StaticVars.VerifyMailPassword);

                var mailBody = Messages.VerifyMailTemplate.Replace("{{code}}", code.ToString()).Replace("{{displayName}}", $"{hasCustomer.FirstName} {hasCustomer.LastName}").Replace("{{year}}", DateTime.Now.Year.ToString());
                var result = await mail.SendMailAsync(dto.Email, StaticVars.VerifyMailUserName, "Karekodla", mailBody, "Karekodla – Hesabını Doğrula", true);
                var customerQr = new CustomerQrVerification
                {
                    CustomerId = hasCustomer.Id,
                    Code = code,
                    ExpireDate = DateTime.Now.AddMinutes(5),
                    CreatedDate = DateTime.Now,
                    VerificationTypeId = 1,
                };
                _customerQrVerificationDal.Add(customerQr);


                return new Result(true, returnMessage);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);

            }
        }

        public IResult Delete(int id)
        {
            var customer = _customerDal.Get(c => c.Id == id);
            customer.IsDeleted = false;
            _customerDal.Update(customer);
            return new Result(true, Messages.CustomerDeleted);
        }
        public IDataResult<List<Customer>> GetAll()
        {
            var customerList = _customerDal.GetList(c => c.IsActive && !c.IsDeleted);
            return new SuccessDataResult<List<Customer>>(customerList, Messages.CustomerGetList);
        }

        public async Task<IResult> SendMailCode(string email)
        {
            var customer = _customerDal.GetList(c => c.Email == email && !c.IsDeleted).LastOrDefault();
            if (customer == null)
                return new Result(false, Messages.UserNotFound);
            var code = ProExt.NextSixDigitCode();

            MailWorker mail = new MailWorker();

            mail.Init(StaticVars.VerifyMailUserName, StaticVars.VerifyMailPassword);

            var mailBody = Messages.VerifyMailTemplate.Replace("{{code}}", code.ToString()).Replace("{{displayName}}", $"{customer.FirstName} {customer.LastName}").Replace("{{year}}", DateTime.Now.Year.ToString());
            var result = await mail.SendMailAsync(email, StaticVars.VerifyMailUserName, "Karekodla", mailBody, "Karekodla – Hesabını Doğrula", true);
            var customerQr = new CustomerQrVerification
            {
                CustomerId = customer.Id,
                Code = code,
                ExpireDate = DateTime.Now.AddMinutes(5),
                CreatedDate = DateTime.Now,
                VerificationTypeId = 1,
            };
            _customerQrVerificationDal.Add(customerQr);

            return new Result(true, Messages.SendMailCode);
        }

        public IDataResult<int> VerifyCustomer(VerifyCustomerEmailDto dto)
        {
            var customer = _customerDal.GetList(c => c.Email == dto.Email && !c.IsDeleted).LastOrDefault();
            if (customer == null)
                return new ErrorDataResult<int>(message: Messages.UserNotFound);

            var code = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > DateTime.Now);
            if (code == null)
                return new ErrorDataResult<int>(message: Messages.VerifyCodeExpired);

            code.VerifyDate = DateTime.Now;
            _customerQrVerificationDal.Update(code);
            customer.IsActive = true;
            _customerDal.Update(customer);

            string directory = Path.Combine(@"D:\vhosts\karekodla.com\UploadFiles\Contracts", $"{customer.FirstName}_{customer.LastName}_{customer.Id}");

            IHtmlToPdf htmlToPdf = new HtmlToPdf(_converter);

            string CreateContract(string contractName, string explanation)
            {
                return Messages.ContractTemplate
                    .Replace("{{KnowledgeBaseName}}", contractName)
                    .Replace("{{Explanation}}", explanation)
                    .Replace("{{NameSurname}}", $"{customer.FirstName} {customer.LastName}")
                    .Replace("{{ApprovalDate}}", $"{code.VerifyDate:dd.MM.yyyy HH:mm}")
                    .Replace("{{IP}}", GetClientIp())
                    .Replace("{{OTP}}", code.Code.ToString());
            }

            void SaveContract(string contractName, string explanation)
            {
                var contractText = CreateContract(contractName, explanation);
                var fileName = $"{contractName}_{customer.FirstName}_{customer.LastName}.pdf";
                var formFile = htmlToPdf.ConvertHtml(contractText, "");
                WriteFile.CreateFileFromBytes(new CreateFileByteWithFileNameDto
                {
                    File = formFile,
                    FolderPath = directory,
                    FileName = fileName
                });

                var customerContract = new CustomerContract
                {
                    CustomerId = customer.Id,
                    ContractName = contractName,
                    FileName = fileName,
                    CreatedDate = code.VerifyDate.Value,
                    VerifyDate = code.VerifyDate.Value,
                    VerifyState = 1,
                    VerifyCode = code.Code,
                    IsActive = true,
                };
                _customerContractDal.Add(customerContract);
            }

            var contractKvkk = _paramItemDal.Get(c => c.ParamId == ParamEnums.Contracts && c.Value == 1 && c.IsActive);

            var contractExplicit = _paramItemDal.Get(c => c.ParamId == ParamEnums.Contracts && c.Value == 2 && c.IsActive);

            SaveContract("KVKK Aydınlatma Metni", contractKvkk.Name);
            SaveContract("Açık Rıza Metni", contractExplicit.Name);

            return new SuccessDataResult<int>(customer.Id, Messages.AccountVerifySuccess);

            string GetClientIp()
            {
                try
                {
                    var ctx = _httpContextAccessor.HttpContext;
                    if (ctx == null) return "";
                    var cf = ctx.Request.Headers["CF-Connecting-IP"].ToString();
                    if (IPAddress.TryParse(cf, out _)) return cf;
                    var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
                    if (!string.IsNullOrWhiteSpace(xff))
                    {
                        var ip = xff.Split(',').Select(s => s.Trim()).FirstOrDefault();
                        if (IPAddress.TryParse(ip, out _)) return ip!;
                    }
                    var xri = ctx.Request.Headers["X-Real-IP"].ToString();
                    if (IPAddress.TryParse(xri, out _)) return xri;
                    return ctx.Connection.RemoteIpAddress?.ToString() ?? "";
                }
                catch { return ""; }
            }
        }
    }
}
