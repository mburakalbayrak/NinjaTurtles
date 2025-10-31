using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : Controller
    {
        private IQrService _qrService;

        public QrController(IQrService qrService)
        {
            _qrService = qrService;
        }

        [HttpGet("[action]")]
        public IActionResult GetQrDetail(Guid id)
        {
            var qr = _qrService.GetQrDetail(id);
            return Ok(qr);
        }


        [HttpPost("[action]")]
        public IActionResult CreateHumanDetail([FromBody] QrCodeHumanCreateDto dto)
        {
            var result = _qrService.CreateHumanDetail(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CreateAnimalDetail([FromBody] QrCodeAnimalCreateDto dto)
        {
            var result = _qrService.CreateAnimalDetail(dto);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult GetHumanDetailVerify(QrUpdateVerifyDto dto)
        {
            var qr = _qrService.GetHumanDetailVerify(dto);
            return Ok(qr);
        }

        [HttpGet("[action]")]
        public IActionResult GetAnimalDetailVerify(QrUpdateVerifyDto dto)
        {
            var qr = _qrService.GetAnimalDetailVerify(dto);
            return Ok(qr);
        }
        [HttpPost("[action]")]
        public IActionResult UpdateHumanDetail([FromBody] QrCodeHumanUpdateDto dto)
        {
            var result = _qrService.UpdateHumanDetail(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateAnimalDetail([FromBody] QrCodeAnimalUpdateDto dto)
        {
            var result = _qrService.UpdateAnimalDetail(dto);
            return Ok(result);
        }

        //[HttpGet("[action]")]
        //public IActionResult GetCurrentDirectory()
        //{
        //    var currentdirectory = Directory.GetCurrentDirectory();
        //    return Ok(currentdirectory);
        //}
    }
}
