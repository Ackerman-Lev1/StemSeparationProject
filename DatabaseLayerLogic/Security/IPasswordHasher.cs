namespace DatabaseLayerLogic.Security
{
    public interface IPasswordHasher
    {
        string GenerateSalt();
        string GeneratePasswordHash(string password, string salt, int iterations = 100_000);
        bool VerifyPassword(string inputPassword, string storedSalt, string storedHash, int iterations = 100_000);
    }
}
