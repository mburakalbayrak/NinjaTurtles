using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ICompanyService
    {
        IResult Add(AddCompanyDto dto);
        Task<IResult> AddDetail(AddCompanyOrderDetailDto dto,string path);
        IDataResult<List<CompanyOrderResponseDto>> GetList();

    }
}
