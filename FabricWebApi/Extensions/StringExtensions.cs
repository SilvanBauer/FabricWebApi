using System.Security.Cryptography;
using System.Text;

namespace FabricWebApi.Extensions
{
    public static class StringExtensions
    {
        public static string CreateSHA256Hash(this string value)
        {
            var hashedPassword = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hashedPassword);
        }
    }
}
