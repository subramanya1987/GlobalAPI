using GlobalAPIServices.Domain.Model.Authentication.Login;
using System.Security.Claims;

namespace GlobalAPIServices.Security
{

    public class TokenResult
    {
        public string? Token { get; set; }       
    }

    public interface ITokenGenerator
    {
        TokenResult GenerateTokenString(UserModel user);
        
    }
}
