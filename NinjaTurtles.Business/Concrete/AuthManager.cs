using Azure.Core;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Entities.Concrete;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Core.Utilities.Security.Hashing;
using NinjaTurtles.Core.Utilities.Security.Jwt;
using NinjaTurtles.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<Core.Utilities.Security.Jwt.AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            
            // Refresh token'ı kullanıcıya kaydet
            user.RefreshToken = accessToken.RefreshToken;
            user.RefreshTokenExpiry = accessToken.RefreshTokenExpiration;
            _userService.UpdateRefreshToken(user);
            
            return new SuccessDataResult<Core.Utilities.Security.Jwt.AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null) 
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt)) 
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);

        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User 
            { 
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 0,
                CreatedDate = DateTime.Now,
            };

            _userService.Add(user);

            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null) 
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }

            return new SuccessResult();
        }

        public IDataResult<Core.Utilities.Security.Jwt.AccessToken> RefreshToken(string refreshToken)
        {
            var user = _userService.GetByRefreshToken(refreshToken);
            
            if (user == null)
            {
                return new ErrorDataResult<Core.Utilities.Security.Jwt.AccessToken>("Geçersiz refresh token");
            }

            if (user.RefreshTokenExpiry < DateTime.Now)
            {
                return new ErrorDataResult<Core.Utilities.Security.Jwt.AccessToken>("Refresh token süresi dolmuş");
            }

            return CreateAccessToken(user);
        }

        public IResult RevokeToken(string refreshToken)
        {
            var user = _userService.GetByRefreshToken(refreshToken);
            
            if (user == null)
            {
                return new ErrorResult("Geçersiz refresh token");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            _userService.UpdateRefreshToken(user);

            return new SuccessResult("Token iptal edildi");
        }
    }
}
