using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Entities.Dtos;
using System.Threading.Tasks;

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
        public IActionResult Add([FromBody] AddCompanyDto dto)
        {
            var result = _companyService.Add(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody] UpdateCompanyDto dto)
        {
            var result = _companyService.Update(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCompanyOrder([FromBody] AddCompanyOrderDetailDto dto)
        {
          
            var result = await _companyService.AddDetail(dto);
            return Ok(result);
        }


    }
}
