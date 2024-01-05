using GlobalAPIServices.Application.Service;
using GlobalAPIServices.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GlobalAPIServices.Controllers
{
    public class UserController : BaseApiController
    {

        private readonly IUserApplicationService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserApplicationService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse>> GetAllUsers()
        {
            var res = await _userService.GetAllUser();

            return new ServiceResponse(res.Item1, res.Item2, JsonConvert.SerializeObject(res.Item3));
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<ServiceResponse>> GetUserById(Guid id)
        {
            string applicationId = _configuration.GetSection("AppSettings:applicationId").Value;
            var res = await _userService.GetUserDetailsById(new Guid(applicationId), id);
            if (res.Item1==200) 
            { 
               return new ServiceResponse(res.Item1,res.Item2, JsonConvert.SerializeObject(res.Item3));
            }
            else
            {
                return BadRequest(new ServiceResponse(res.Item1,res.Item2,data:string.Empty));
            }
        }
    }
}
