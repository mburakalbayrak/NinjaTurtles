using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Entities.Dtos;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner
        /// </summary>
        /// <param name="userForLoginDto">Email ve şifre bilgileri</param>
        /// <returns>Access Token ve Refresh Token</returns>
        /// <response code="200">Login başarılı - Token'lar döner</response>
        /// <response code="400">Login başarısız - Geçersiz kimlik bilgileri</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);

            if (!userToLogin.Success) 
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);

            if (result.Success) 
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Data);
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="userForRegisterDto">Kullanıcı kayıt bilgileri</param>
        /// <returns>Access Token ve Refresh Token</returns>
        /// <response code="200">Kayıt başarılı - Token'lar döner</response>
        /// <response code="400">Kayıt başarısız - Kullanıcı zaten mevcut veya geçersiz bilgi</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);

            if (!userExists.Success) 
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            var result = _authService.CreateAccessToken(registerResult.Data);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Refresh token ile yeni access token alır
        /// </summary>
        /// <param name="refreshTokenDto">Refresh token bilgisi</param>
        /// <returns>Yeni Access Token ve Refresh Token</returns>
        /// <response code="200">Token yenileme başarılı</response>
        /// <response code="400">Geçersiz veya süresi dolmuş refresh token</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = _authService.RefreshToken(refreshTokenDto.RefreshToken);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Kullanıcı çıkışı yapar ve refresh token'ı iptal eder
        /// </summary>
        /// <param name="refreshTokenDto">İptal edilecek refresh token</param>
        /// <returns>Başarı mesajı</returns>
        /// <response code="200">Logout başarılı - Token iptal edildi</response>
        /// <response code="400">Geçersiz refresh token</response>
        /// <response code="401">Yetkilendirme gerekli</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Logout([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = _authService.RevokeToken(refreshTokenDto.RefreshToken);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
