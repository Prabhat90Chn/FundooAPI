using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserEntity RegisterUser(EntityModel userRegistration);
        public string LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string  email);
        public bool ResetPassword(string newPassword, int userId);
    }
}
