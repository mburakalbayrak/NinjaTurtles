using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ICompanyService
    {
        IResult Add(AddCompanyDto dto);
        IResult Update(UpdateCompanyDto dto);
        Task<IResult> AddDetail(AddCompanyOrderDetailDto dto);
        IDataResult<List<CompanyOrderResponseDto>> GetList();

    }
}
