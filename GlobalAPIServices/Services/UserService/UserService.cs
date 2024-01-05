using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GlobalAPIServices.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor=httpContextAccessor;
        }
        public string GetMyName()
        {
            var resutlt=string.Empty;
            if(_httpContextAccessor.HttpContext!=null)
            {
                resutlt=_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);               
            }
            return resutlt;
        }
    }
}
