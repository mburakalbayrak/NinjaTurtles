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

        public IResult Add(AddCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);
            _customerDal.Add(customer);
            return new Result(true, Messages.CustomerAdded);
        }

        public IResult Delete(int id)
        {
            var customer =_customerDal.Get(c=>c.Id == id);
            customer.IsActive = false;
            _customerDal.Update(customer);
            return new Result(true, Messages.CustomerDeleted);
        }
    }
}
