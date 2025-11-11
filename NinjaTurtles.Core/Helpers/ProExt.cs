using System.Security.Cryptography;

namespace NinjaTurtles.Core.Helpers
{
    public class ProExt
    {

        public static int NextSixDigitCode()
        {
            int value = RandomNumberGenerator.GetInt32(0, 1_000_000); 
            return value;
        }

        public static string GenerateBase64WithReplace()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("+", "").Replace("-", "").Replace("%", "").Replace(" ", "");
        }
    
    }
}
