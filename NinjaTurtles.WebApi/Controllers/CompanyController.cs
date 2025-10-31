using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private ICompanyService _companyService;

        public CompanyController(ICompanyService companyOrderService)
        {
            _companyService = companyOrderService;
        }

        [HttpGet("getList")]
        public IActionResult GetList()
        {
            var result = _companyService.GetList();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] string name)
        {
            var result =_companyService.Add(name);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult AddCompanyOrder([FromBody] AddCompanyOrderDetailDto dto)
        {
            var currentdirectory = Directory.GetCurrentDirectory();
            var result = _companyService.AddDetail(dto,currentdirectory);
            return Ok(result);
        }

      
    }
}
