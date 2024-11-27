using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Helpers.QrCode;
using NinjaTurtles.Core.NetCoreConfiguration;
using NinjaTurtles.Entities.Config;
using NinjaTurtles.Entities.Dtos;
using System.Drawing.Imaging;

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
        [Authorize(Roles = "Category.List")]
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddCustomerDto dto)
        {
            var result =_customerService.Add(dto);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _customerService.Delete(id);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult SaveQrCode([FromBody] string url)
        {
            var filePath = $"D:/UploadFiles/QrCode/{url}.png";
            var result = QRCodeHelper.SaveQRCodeAsImage(url,filePath,ImageFormat.Png);
            return Ok(result);
        }
    }
}
