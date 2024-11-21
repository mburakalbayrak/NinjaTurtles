using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Utilities.Security.Jwt
{
    public class TokenOptions
    {
        public string Audience { get; set; } // Tokenın kullanıcı kitlesi
        public string Issuer { get; set; } // İmzalayan
        public int AccessTokenExpiration { get; set; } // Token Süresi
        public string SecurityKey { get; set; }
    }
}
