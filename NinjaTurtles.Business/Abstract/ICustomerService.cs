﻿using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ICustomerService
    {
        IResult Add(AddCustomerDto dto);
        IResult Delete(int id);
        IDataResult<List<Customer>> GetAll();
    }
}
