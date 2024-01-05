using GlobalAPIServices.Domain.Model.Authentication.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAPIServices.Infrastracture.Repository
{
    public interface IUserRepository
    {
        Task<Tuple<int, string>> CreateUser(UserModel userModel);
        Task<Tuple<int, string, List<UserModel>>> GetAllUser();
        Task<Tuple<int, string, UserModel>> GetUserDetailsById(Guid applicationId, Guid id);
        Task<Tuple<int, string, UserModel>> GetUserDetailsByUserName(string userName);
        Task<Tuple<int, string, bool>> IsUserExist(string userName);
        Task<Tuple<int, string>> IsValidResetPassword(ResetPasswordModel resetPassword);
        Task<Tuple<int, string, Guid>> IsValidUser(string userName, string password);
        Task<Tuple<int, string>> ResetPassword(ResetPasswordModel resetPassword);
        Task<Tuple<int, string>> UpdateUser(UserModel userModel);


    }
}
