using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Entities.Dtos;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetQrDetail(Guid id)
        {
            var qr = await _qrService.GetQrDetail(id);
            return Ok(qr);
        }


        [HttpPost("[action]")]
        public IActionResult CreateRedirectUrl([FromBody] QrRedirectUrlDto dto)
        {
            var result = _qrService.CreateRedirectUrl(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult CreateHumanDetail([FromForm] QrCodeHumanCreateDto dto)
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
        public IActionResult GetHumanDetailVerify([FromQuery] QrUpdateVerifyDto dto)
        {
            var qr = _qrService.GetHumanDetailVerify(dto);
            return Ok(qr);
        }

        [HttpGet("[action]")]
        public IActionResult GetAnimalDetailVerify([FromQuery] QrUpdateVerifyDto dto)
        {
            var qr = _qrService.GetAnimalDetailVerify(dto);
            return Ok(qr);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateRedirectUrl([FromForm] QrRedirectUrlUpdateDto dto)
        {
            var result = _qrService.UpdateRedirectUrl(dto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult UpdateHumanDetail([FromForm] QrCodeHumanUpdateDto dto)
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

        [HttpGet("[action]")]
        public IActionResult GetCurrentDirectory()
        {
            var currentdirectory = Directory.GetCurrentDirectory();
            return Ok(currentdirectory);
        }
    }
}
