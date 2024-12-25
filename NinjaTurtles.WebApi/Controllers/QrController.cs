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
        public IActionResult MailTest()
        {
            MailWorker mail= new MailWorker();
            mail.Init("mail.kurumsaleposta.com",587, "dogrula@karekodla.com.tr", "5Z1Kp3o:Fc_kM=-6",true);
          var result=  mail.SendMail("yusuf.celik@karekodla.com.tr", "dogrula@karekodla.com.tr", "345631", "Hesap Doğrulama", true);
            return Ok(result);
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
        public IActionResult GetCurrentDirectory()
        {
            var currentdirectory = Directory.GetCurrentDirectory();
            return Ok(currentdirectory);
        }
    }
}
