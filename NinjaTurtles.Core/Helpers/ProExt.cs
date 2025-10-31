using System.Security.Cryptography;

namespace NinjaTurtles.Core.Helpers
{
    public class ProExt
    {
      
        public static long GenerateAccountNumber(String startWith = "32", string numLength = "6")
        {
            Random generator = new Random();
            String createNumber = generator.Next(0, 999999).ToString("D" + numLength);
            return Convert.ToInt64(startWith + createNumber);
        }

        public static string GenerateBase64WithReplace()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("+", "").Replace("-", "").Replace("%", "").Replace(" ", "");
        }
    
    }
}
