using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;

using NinjaTurtles.Entities.Dtos;


namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getList")]
        [Authorize(Roles = "Customer.List")]
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody] AddCustomerDto dto)
        {
            var result = await _customerService.Add(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult VerifyCustomer([FromBody] VerifyCustomerEmailDto dto)
        {
            var result = _customerService.VerifyCustomer(dto);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SendMailCode(string email)
        {
            var result = await _customerService.SendMailCode(email);
            return Ok(result);
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            var result = _customerService.Delete(id);
            return Ok(result);
        }

    
    }
}
