using Azure.Core;
using NinjaTurtles.Core.Entities.Concrete;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.Core.Utilities.Security.Jwt;
using NinjaTurtles.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);

        IResult UserExists(string email);

        IDataResult<Core.Utilities.Security.Jwt.AccessToken> CreateAccessToken(User user);
        IDataResult<Core.Utilities.Security.Jwt.AccessToken> RefreshToken(string refreshToken);
        IResult RevokeToken(string refreshToken);
    }
}
