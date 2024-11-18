using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ICustomerService
    {
        IResult Add(AddCustomerDto dto);
        IResult Delete(int id);
    }
}
