using GlobalAPIServices.Application.Service;
using GlobalAPIServices.Domain.Model.Authentication.Login;
using GlobalAPIServices.Infrastracture.Repository;
namespace GlobalAPIServices.Application.Impl
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserRepository _userRepository;
        public UserApplicationService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async  Task<Tuple<int, string>> CreateUser(UserModel userModel)
        {
            return await _userRepository.CreateUser(userModel);
        }

        public async Task<Tuple<int, string, List<UserModel>>> GetAllUser()
        {
            return await _userRepository.GetAllUser();           
        }

        public async Task<Tuple<int, string, UserModel>> GetUserDetailsById(Guid applicationId, Guid id)
        {
            return await _userRepository.GetUserDetailsById(applicationId, id);
        }

        public async Task<Tuple<int, string, UserModel>> GetUserDetailsByUserName(string userName)
        {
            return await _userRepository.GetUserDetailsByUserName(userName);
        }

        public async Task<Tuple<int, string, bool>> IsUserExist(string userName)
        {
            return await _userRepository.IsUserExist(userName);
        }

        public async Task<Tuple<int, string>> IsValidResetPassword(ResetPasswordModel resetPassword)
        {
            return await _userRepository.IsValidResetPassword(resetPassword);
        }

        public async Task<Tuple<int, string, Guid>> IsValidUser(string userName, string password)
        {
            return await _userRepository.IsValidUser(userName, password);
        }

        public Task<Tuple<int, string>> ResetPassword(ResetPasswordModel resetPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<int, string>> UpdateUser(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
