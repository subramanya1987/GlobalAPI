namespace GlobalAPIServices.Security
{
    public interface IPasswordManager
    {
        List<byte[]>  CreatePasswordHash(string password);

        bool VerifyPasswordHash(string password, byte[] passworHash, byte[] passwordSalt);

        string HashPassword(string password);

        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}
