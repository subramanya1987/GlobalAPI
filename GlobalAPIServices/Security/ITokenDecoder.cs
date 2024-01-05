namespace GlobalAPIServices.Security
{
    public interface ITokenDecoder
    {
        string Decrypt(string encodedToken);
    }
}
