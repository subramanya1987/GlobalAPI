using GlobalAPIServices.Domain.Model.Authentication.Login;
using GlobalAPIServices.Infrastracture.Repository.UserManagementData;
using GlobalAPIServices.Model.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GlobalAPIServices.Security
{
    public class TokenGenerator : ITokenGenerator
    {

        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  TokenResult GenerateTokenString(UserModel user)
        {
            return CreateToken(user);
        }

        private  TokenResult CreateToken(UserModel user)
        {
            string userName=string.Empty;
            if(!string.IsNullOrEmpty(user.UserName))
            {
                userName=user.UserName;
            }
            List<Claim> claims = new List<Claim>
            {
              
                     new Claim(ClaimTypes.Name,userName),
                     new Claim(ClaimTypes.Role,"admin")                
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding
                .UTF8.GetBytes(_configuration.GetSection("AppSettings:token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(1)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var res = new TokenResult { 
                Token = jwt
            };
            return res;
        }

        
    }
}
