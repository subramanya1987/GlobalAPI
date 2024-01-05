using GlobalAPIServices.Domain.Model.Authentication.Login;
using GlobalAPIServices.Infrastracture.Repository.UserManagementData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GlobalAPIServices.Infrastracture.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementContext _context;
        public UserRepository(UserManagementContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int, string>> CreateUser(UserModel userModel)
        {
#pragma warning disable CS8601,CS8602,8604

            if(!IsUserNotExist(userModel.UserName,userModel.Email,userModel.PhoneNumber))
            {
                return new Tuple<int, string>(400, "User alreay exist");
            }

            User obj = new User
            {
                Id = Guid.NewGuid(),
                ApplicationId = userModel.ApplicationId,
                UserName = userModel.UserName,
                NormalizedUserName = Convert.ToString(userModel.UserName).ToUpper(),
                Email = userModel.Email,
                EmailConfirmed=false,
                NormalizedEmail = Convert.ToString(userModel.Email).ToUpper(),
                Password = userModel.Password,
                PasswordHash = userModel.PasswordHash,
                PhoneNumber = userModel.PhoneNumber,
                PhoneNumberConfirmed=false,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                ProfilePicture = userModel.ProfilePicture,
                Gender = userModel.Gender,
                ProfilePicturePath = userModel.ProfilePicturePath,
                AccessFailedCount = 0,
                IsActive = true,
                CreatedBy = userModel.UserName,
                CreatedDate = DateTime.Now
            };
            if (userModel.DateOfBirth != null)
            {
                obj.Age = CalculateAge(Convert.ToDateTime(userModel.DateOfBirth), DateTime.Now);
                obj.DateOfBirth = userModel.DateOfBirth;
            }
#pragma warning restore CS8601, CS8602, 8604

            _context.Users.Add(obj);
            await _context.SaveChangesAsync();
            return new Tuple<int, string>(200, "Success");
        }

        public async Task<Tuple<int, string, List<UserModel>>> GetAllUser()
        {
            var res = await _context.Users.OrderBy(xx => xx.FirstName).Select(x => new UserModel
            {
                Id = x.Id,
                ApplicationId = x.ApplicationId,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                TwoFactorEnabled = x.TwoFactorEnabled,
                AccessFailedCount = x.AccessFailedCount,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ProfilePicture = x.ProfilePicture,
                UsernameChangeLimit = x.UsernameChangeLimit,
                Age = x.Age,
                Gender = x.Gender
            }).ToListAsync();

            return new Tuple<int, string, List<UserModel>>(200, "Success", res);
        }

        public async Task<Tuple<int, string, UserModel>> GetUserDetailsById(Guid applicationId, Guid id)
        {
            var res = await _context.Users
                .Where(xx => xx.ApplicationId == applicationId && xx.Id == id)
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    ApplicationId = x.ApplicationId,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed,
                    PhoneNumber = x.PhoneNumber,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    TwoFactorEnabled = x.TwoFactorEnabled,
                    AccessFailedCount = x.AccessFailedCount,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ProfilePicture = x.ProfilePicture,
                    UsernameChangeLimit = x.UsernameChangeLimit,
                    Age = x.Age,
                    Gender = x.Gender
                }).FirstOrDefaultAsync();
            if (res != null)
            {
                return new Tuple<int, string, UserModel>(200, "Success", res);
            }
            else
            {
                return new Tuple<int, string, UserModel>(400, "Invalid UserId", new UserModel());
            }
        }

        public async Task<Tuple<int, string, UserModel>> GetUserDetailsByUserName(string userName)
        {
            var res = await _context.Users
               .Where(xx => xx.UserName == userName)
               .Select(x => new UserModel
               {
                   Id = x.Id,
                   ApplicationId = x.ApplicationId,
                   UserName = x.UserName,
                   NormalizedUserName = x.NormalizedUserName,
                   Email = x.Email,
                   NormalizedEmail = x.NormalizedEmail,
                   EmailConfirmed = x.EmailConfirmed,
                   PhoneNumber = x.PhoneNumber,
                   PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                   TwoFactorEnabled = x.TwoFactorEnabled,
                   AccessFailedCount = x.AccessFailedCount,
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   ProfilePicture = x.ProfilePicture,
                   UsernameChangeLimit = x.UsernameChangeLimit,
                   Age = x.Age,
                   Gender = x.Gender,
                   DateOfBirth = x.DateOfBirth
               }).FirstOrDefaultAsync();

            if (res != null)
            {
                return new Tuple<int, string, UserModel>(200, "Success", res);
            }
            else
            {
                return new Tuple<int, string, UserModel>(400, "Invalid UserId", new UserModel());
            }
        }

        public async Task<Tuple<int, string, bool>> IsUserExist(string userName)
        {
            var res = await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            if (res == null)
            {
                return Tuple.Create(200, "Success", true);
            }
            else
            {
                return Tuple.Create(400, "User already exist", false);
            }
        }

        public async Task<Tuple<int, string>> IsValidResetPassword(ResetPasswordModel resetPassword)
        {
            var resObj = await _context.Users.Where(x => x.Id == resetPassword.Id).FirstOrDefaultAsync();
            if (resObj != null)
            {
                if (resObj.PasswordHash != resetPassword.PasswordHash)
                {
                    return new Tuple<int, string>(200, "Success");
                }
                else
                {
                    return new Tuple<int, string>(400, "Old and New password should not same");
                }
            }
            return new Tuple<int, string>(400, "Invalid User Id");
        }

        public async Task<Tuple<int, string, Guid>> IsValidUser(string userName, string password)
        {
            var resUser = await _context.Users.Where(x => x.UserName == userName).FirstOrDefaultAsync();
            if (resUser != null)
            {
#pragma warning disable CS8604 
                if (VerifyHashedPassword(resUser.PasswordHash, password))
                {
                    return Tuple.Create(200, "Success", resUser.Id);
                }
                else
                {
                    int accessFailedCount = Convert.ToInt32(resUser.AccessFailedCount);
                    resUser.AccessFailedCount = accessFailedCount += 1;
                    await _context.SaveChangesAsync();
                    return Tuple.Create(400, "User Name or Password Invalid", new Guid());
                }
#pragma warning restore CS8604
            }
            else
            {
                return Tuple.Create(400, "User Name or Password Invalid", new Guid());

            }
        }

        public async Task<Tuple<int, string>> ResetPassword(ResetPasswordModel resetPassword)
        {
            var resObj = await _context.Users.Where(x => x.Id == resetPassword.Id).FirstOrDefaultAsync();
#pragma warning disable CS8601,CS8602
            resObj.PasswordHash = resetPassword.PasswordHash;
            resObj.Password = resetPassword.Password;
#pragma warning restore CS8601, CS8602
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(200, "Success");
        }

        public async Task<Tuple<int, string>> UpdateUser(UserModel userModel)
        {
            var resObj = await _context.Users.Where(x => x.Id == userModel.Id).FirstOrDefaultAsync();
            
            if (resObj != null) 
            {
                resObj.FirstName = userModel.FirstName;
                resObj.LastName = userModel.LastName;   
                resObj.Gender = userModel.Gender;
                if(resObj.DateOfBirth != userModel.DateOfBirth)
                {
                    resObj.Age = CalculateAge(Convert.ToDateTime(userModel.DateOfBirth), DateTime.Now);
                    resObj.DateOfBirth = userModel.DateOfBirth;
                }
                if(userModel.ProfilePicture != null)
                {
                    resObj.ProfilePicturePath = userModel.ProfilePicturePath;
                    resObj.ProfilePicture = userModel.ProfilePicture;
                }
                if(resObj.Email!=userModel.Email)
                {
                    resObj.EmailConfirmed = false;
                    resObj.Email = userModel.Email;
                }
                
                if(resObj.PhoneNumber!=userModel.PhoneNumber)
                {
                    resObj.PhoneNumberConfirmed = false;
                    resObj.PhoneNumber = userModel.PhoneNumber;
                }
                resObj.ModifiedBy = userModel.UserName;
                resObj.ModifiedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return new Tuple<int, string>(200, "Success");
            }
            return new Tuple<int, string>(400, "Invalid User Id");
        }

        #region Internal Private Methods

        private bool IsUserNotExist(string username,string emailId,string phoneNumber)
        {
            var res = _context.Users.Where(x => x.UserName == username || x.Email == emailId || x.PhoneNumber == phoneNumber).FirstOrDefault();
            if (res == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int CalculateAge(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        #endregion
    }
}
