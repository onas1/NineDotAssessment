using System.Security.Cryptography;
using System.Text;

namespace NineDotAssessment.Application.Common.Helpers
{
    public static partial class Utilities
    {
        // Helper to create a hashed PIN
        public static (string Hash, string Salt) CreateCredential(string credential)
        {
            using var rng = new RNGCryptoServiceProvider();

            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);
            // Hash the PIN with the salt
            string hash = ComputeHash(credential, salt);

            return (Hash: hash, Salt: salt);
        }

        private static string ComputeHash(string pin, string salt)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            byte[] pinBytes = Encoding.UTF8.GetBytes(pin);
            byte[] hashBytes = hmac.ComputeHash(pinBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString(); // 4-digit OTP
        }

        public static bool VerifyCredential(string credential, string storedHash, string storedSalt)
        {
            string computedHash = ComputeHash(credential, storedSalt);
            // Compare the computed hash with the stored hash
            return computedHash == storedHash;
        }


    }
}
