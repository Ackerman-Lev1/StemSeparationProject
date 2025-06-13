using System.Security.Cryptography;

namespace DatabaseLayerLogic.Security
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        public string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(salt);
        }

        public string GeneratePasswordHash(string password, string salt, int iterations = 100_100)
        {
            var saltBytes = Convert.FromBase64String(salt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, iterations , HashAlgorithmName.SHA512, 32);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string inputPassword, string storedSalt, string storedHash, int iterations = 100_000)
        {
            string hashOfInput = GeneratePasswordHash(inputPassword, storedSalt, iterations);

            byte[] inputBytes = Convert.FromBase64String(hashOfInput);
            byte[] storedBytes = Convert.FromBase64String(storedHash);

            return CryptographicOperations.FixedTimeEquals(inputBytes, storedBytes);
        }
    }
}
