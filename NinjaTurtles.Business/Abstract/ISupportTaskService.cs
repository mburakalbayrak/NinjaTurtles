using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.Business.Abstract
{
    public interface ISupportTaskService
    {
        Task<IResult> SendReport(SendReportDto dto);
    }
}
