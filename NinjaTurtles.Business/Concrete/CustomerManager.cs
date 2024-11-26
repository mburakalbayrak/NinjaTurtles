using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;
        private IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IResult Add(AddCustomerDto dto)
        {
            //var customerData = _mapper.Map<Customer>(dto);

            var customerData = new Customer
            {
                CreatedBy = 1, // TO:DO
                CreatedDate = DateTime.Now,
                Email = dto.EmailName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                IsActive = true,
                IsDeleted = false,
                PhoneNumber = dto.PhoneNumber,

            };

            _customerDal.Add(customerData);
            return new Result(true, Messages.CustomerAdded);
        }

        public IResult Delete(int id)
        {
            var customer =_customerDal.Get(c=>c.Id == id);
            customer.IsDeleted = false;
            _customerDal.Update(customer);
            return new Result(true, Messages.CustomerDeleted);
        }
        public IDataResult<List<Customer>> GetAll()
        {
            var customerList = _customerDal.GetList(c => c.IsActive && !c.IsDeleted);
            return new SuccessDataResult<List<Customer>>(customerList, Messages.CustomerGetList);
        }
    }
}
