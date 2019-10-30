using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Utilities
{
    public static class HashUtility
    {
        private static readonly int MaxSaltLength = 32;

        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            using (HashAlgorithm algorithm = new SHA256Managed())
            {
                byte[] plainTextWithSaltBytes = plainText.Concat(salt).ToArray();

                return algorithm.ComputeHash(plainTextWithSaltBytes);
            }
        }

        public static string GenerateSaltedHash(string plainText, string salt)
        {
            byte[] result = GenerateSaltedHash(Encoding.ASCII.GetBytes(plainText), Encoding.ASCII.GetBytes(salt));
            return Convert.ToBase64String(result);
        }

        public static string GenerateSalt()
        {
            using (RandomNumberGenerator random = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[MaxSaltLength];
                random.GetNonZeroBytes(salt);
                return Encoding.ASCII.GetString(salt);
            }
        }
    }
}
