using GlobalAPIServices.Application.Service;
using GlobalAPIServices.Domain.Model.Authentication.Login;
using GlobalAPIServices.Model;
using GlobalAPIServices.Model.Auth;
using GlobalAPIServices.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GlobalAPIServices.Controllers
{
    public class LoginController : BaseApiController
    {
        private readonly IUserApplicationService _userService;
        private readonly IPasswordManager _passwordManager;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApplicationService userService
            , IPasswordManager passwordManager
            , IConfiguration configuration)
        {
            _userService = userService;
            _passwordManager = passwordManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Login(LoginModel loginModel)
        {
            if (loginModel.Username != null && loginModel.Password != null)
            {
                //string passwordHash = GetCreatePasswordHash(loginModel.Password);
                var res = await _userService.IsValidUser(loginModel.Username, loginModel.Password);
                if (res.Item1 == 200)
                {
#pragma warning disable CS8604
                    return new ServiceResponse(res.Item1, res.Item2, data: Convert.ToString(res.Item3));
#pragma warning restore CS8604
                }
                else
                {
                    return new ServiceResponse(res.Item1, res.Item2, data: string.Empty);
                }

            }
            return BadRequest(new ServiceResponse(401, "Invalid User Creadentials", ""));
        }


        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ServiceResponse>> Register(UserRegisterModel userRegister)
        {
            if(userRegister.Password!=userRegister.ConfirmPassword)
            {
                return BadRequest(new ServiceResponse(400, "Password and Confirm Password Schould Same", data:string.Empty));
            }
            var resUserExist = await  _userService.IsUserExist(userRegister.UserName);
            if(resUserExist.Item1!=200)
            {
                return BadRequest(new ServiceResponse(resUserExist.Item1,resUserExist.Item2, JsonConvert.SerializeObject(resUserExist.Item3.ToString())));
            }

            var userModel = ConvertDataFromUserRegisterModelToUserModel(userRegister);
            var res = await  _userService.CreateUser(userModel);
            
            if(resUserExist.Item1!=200)
            {
                return BadRequest(new ServiceResponse(res.Item1,res.Item2, data: string.Empty));
            }
            else
            {
                return new ServiceResponse(res.Item1, res.Item2, data: string.Empty);
            }
        }

        
        #region Internal Private Methods
        private string  GetCreatePasswordHash(string password)
        {
            return _passwordManager.HashPassword(password);  
        }
        private UserModel ConvertDataFromUserRegisterModelToUserModel(UserRegisterModel userRegisterModel)
        {
            var obj = new UserModel();
            
            string  applicationId = _configuration.GetSection("AppSettings:applicationId").Value;
            string  passwordHash = GetCreatePasswordHash(userRegisterModel.Password);
            obj.ApplicationId = new Guid(applicationId);
            obj.PasswordHash = passwordHash;           
            obj.UserName = userRegisterModel.UserName;
            obj.Password = userRegisterModel.Password;
            obj.Email = userRegisterModel.Email;
            obj.PhoneNumber= userRegisterModel.PhoneNumber;
            obj.FirstName = userRegisterModel.FirstName;
            obj.LastName = userRegisterModel.LastName;
            obj.ProfilePicture = userRegisterModel.ProfilePicture;
            obj.ProfilePicturePath=userRegisterModel.ProfilePicturePath;                
            obj.Gender= userRegisterModel.Gender;
            obj.DateOfBirth = userRegisterModel.DateOfBirth;
            return obj;
        }

        #endregion


    }
}
