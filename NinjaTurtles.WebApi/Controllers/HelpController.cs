using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Entities.Dtos;
using System.Threading.Tasks;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : Controller
    {
        private ISupportTaskService _supportTaskService;

        public HelpController(ISupportTaskService supportTaskService)
        {
            _supportTaskService = supportTaskService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendReport([FromBody] SendReportDto dto)
        {
            var result = await _supportTaskService.SendReport(dto);
            return Ok(result);
        }
    }
}
