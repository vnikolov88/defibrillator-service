using System.Security.Cryptography;
using System.Text;

namespace DefibrillatorService.Extensions
{
    public static class CryptoExtensions
    {
        public static byte[] EncodeHMACSHA256(this string text, string key)
        {
            var encoding = new UTF8Encoding();

            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);

            byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return hashBytes;
        }
    }
}
