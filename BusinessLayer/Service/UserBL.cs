using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private  IUserRL userRl;

        public UserBL(IUserRL userRl)
        {
            this.userRl = userRl;
        }
        public UserEntity RegisterUser(EntityModel userModel)
        {
            return userRl.RegisterUser(userModel);
            
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            return userRl.LoginUser(userLoginDto);
        }

        public bool ResetPassword(string newPassword, int userId)
        {
            return userRl.ResetPassword(newPassword,userId);
        }

        public Task<string> ForgetPassword(string email)
        {
            return userRl.ForgetPassword(email);
        }
    }
}
