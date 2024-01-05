using System.IdentityModel.Tokens.Jwt;

namespace GlobalAPIServices.Security
{
    public class TokenDecoder: ITokenDecoder
    { 
        public TokenDecoder() { }

        public string Decrypt(string encodedToken)
        {
            var token = "[encoded jwt]";
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return  jwtSecurityToken.ToString();
        }
    }
}
