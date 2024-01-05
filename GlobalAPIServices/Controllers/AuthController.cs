using GlobalAPIServices.Model.Auth;
using GlobalAPIServices.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GlobalAPIServices.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration,IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet,Authorize]
        public ActionResult<string> GetMe()
        {
            var result= _userService.GetMyName();
            return Ok(result);
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = userDto.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return Ok(user);
        }

        [HttpPost("login")]
        public  ActionResult<string> Login(UserDto userDto)
        {
            if(user.Username != userDto.Username) 
            {
                return BadRequest("User not found.");
            }

            //if(!VerifyPasswordHash(userDto.Password,user.PasswordHash,user.PasswordSalt))
            //{
            //    return BadRequest("Wrong Password.");
            //}

            string token =  CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public ActionResult<string> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if(!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(user.TockenExpiredDateTime<DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            string token=CreateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            return Ok(token);
        }
        private RefreshToken GenerateRefreshToken()
        {
            var tokenExpireSecounds = Convert.ToInt32(_configuration.GetSection("AppSettings:tokenExpireSecounds").Value);
            var refreshToken = new RefreshToken
            {
                RefToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                TockenExpiredDateTime = DateTime.Now.AddSeconds(tokenExpireSecounds),
                TockenCreatedDateTime = DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.TockenExpiredDateTime
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.RefToken, cookieOptions);
            user.RefreshToken = newRefreshToken.RefToken;
            user.TockenCreatedDateTime = newRefreshToken.TockenCreatedDateTime;
            user.TockenExpiredDateTime=newRefreshToken.TockenExpiredDateTime;
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                 new Claim(ClaimTypes.Role,"admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding
                .UTF8.GetBytes(_configuration.GetSection("AppSettings:token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                signingCredentials:credentials,
                expires:DateTime.Now.AddHours(1)
                );
            var jwt= new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passworHash, byte[] passwordSalt )
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passworHash);
            }

        }


    }
}
