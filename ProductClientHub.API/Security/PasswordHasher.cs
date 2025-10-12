using System.Security.Cryptography;

namespace ProductClientHub.API.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static (string Salt, string Hash) Hash(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, KeySize);
            return (Convert.ToBase64String(saltBytes), Convert.ToBase64String(hashBytes));
        }

        public static bool Verify(string password, string saltBase64, string hashBase64)
        {
            var saltBytes = Convert.FromBase64String(saltBase64);
            var computed = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, KeySize);
            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(hashBase64), computed);
        }
    }
}