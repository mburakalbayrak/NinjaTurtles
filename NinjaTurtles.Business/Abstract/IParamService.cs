using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface IParamService
    {
        IDataResult<List<SelectDto>> List(int paramId);
    }
}
