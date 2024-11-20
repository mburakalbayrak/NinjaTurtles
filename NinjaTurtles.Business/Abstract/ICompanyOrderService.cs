using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ICompanyOrderService
    {
        IResult Add(string name);
        IResult AddDetail(AddCompanyOrderDetailDto dto);

    }
}
