using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Entities;
using NinjaTurtles.Core.Helpers;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NinjaTurtles.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;
        private IQrCodeMainDal _qrCodeMainDal;
        private ICustomerQrVerificationDal _customerQrVerificationDal;
        private IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IQrCodeMainDal qrCodeMainDal, ICustomerQrVerificationDal customerQrVerificationDal, IMapper mapper)
        {
            _customerDal = customerDal;
            _qrCodeMainDal = qrCodeMainDal;
            _customerQrVerificationDal = customerQrVerificationDal;
            _mapper = mapper;
        }

        public async Task<IResult> Add(AddCustomerDto dto)
        {
            try
            {
                var returnMessage =Messages.HasCustomer;
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

                var code = Convert.ToInt32(ProExt.GenerateAccountNumber(null, "6"));

                var customerQr = new CustomerQrVerification
                {
                    CustomerId = hasCustomer.Id,
                    Code = code,
                    ExpireDate = DateTime.Now.AddMinutes(5),
                    CreatedDate = DateTime.Now,
                    VerificationTypeId = 1,
                };
                _customerQrVerificationDal.Add(customerQr);
                MailWorker mail = new MailWorker();

                mail.Init("mail.kurumsaleposta.com", 587, "dogrula@karekodla.com.tr", "5Z1Kp3o:Fc_kM=-6", false, false);

                var mailBody = Messages.VerifyMailTemplate.Replace("{{code}}", code.ToString()).Replace("{{displayName}}", $"{hasCustomer.FirstName} {hasCustomer.LastName}").Replace("{{year}}", DateTime.Now.Year.ToString());
                var result = await mail.SendMailAsync(dto.Email, "dogrula@karekodla.com.tr", "Karekodla", mailBody, "Karekodla – Hesabını Doğrula", true);
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
            var customer = _customerDal.GetList(c => c.Email == email  && !c.IsDeleted && c.IsActive).LastOrDefault();
            if (customer==null)
                return new Result(false, Messages.UserNotFound);
            var code = Convert.ToInt32(ProExt.GenerateAccountNumber(null, "6"));
            var customerQr = new CustomerQrVerification
            {
                CustomerId = customer.Id,
                Code = code,
                ExpireDate = DateTime.Now.AddMinutes(5),
                CreatedDate = DateTime.Now,
                VerificationTypeId = 1,
            };
            _customerQrVerificationDal.Add(customerQr);
            MailWorker mail = new MailWorker();

            mail.Init("mail.kurumsaleposta.com", 587, "dogrula@karekodla.com.tr", "5Z1Kp3o:Fc_kM=-6", false, false);

            var mailBody = Messages.VerifyMailTemplate.Replace("{{code}}", code.ToString()).Replace("{{displayName}}", $"{customer.FirstName} {customer.LastName}").Replace("{{year}}", DateTime.Now.Year.ToString());
            var result = await mail.SendMailAsync(email, "dogrula@karekodla.com.tr", "Karekodla", mailBody, "Karekodla – Hesabını Doğrula", true);
            return new Result(true, Messages.SendMailCode);
        }

        public IDataResult<int> VerifyCustomer(VerifyCustomerEmailDto dto)
        {
            var customer = _customerDal.GetList(c => c.Email == dto.Email && !c.IsDeleted).LastOrDefault();
            if (customer == null)
                return new ErrorDataResult<int>(message: Messages.UserNotFound);

            var date = DateTime.Now;
            var code = _customerQrVerificationDal.Get(c => c.Code == dto.Code && c.ExpireDate > date);
            if (code == null)
                return new ErrorDataResult<int>(message: Messages.VerifyCodeExpired);

            customer.IsActive = true;
            _customerDal.Update(customer);

            return new SuccessDataResult<int>(data: customer.Id, message: Messages.AccountVerifySuccess);
        }
    }
}
