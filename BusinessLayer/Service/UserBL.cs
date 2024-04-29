using BusinessLayer.BLException;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.JwtToken;
using RepositoryLayer.RLException;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private IUserRL _userRl;
        private JwtToken _jwtToken;

        public UserBL(IUserRL userRl, JwtToken jwtToken)
        {
            _userRl = userRl;
            _jwtToken = jwtToken;
        }

        public UserModel RegisterUser(RegistrationModel userModel)
        {
            try
            {
                var user = _userRl.RegisterUser(userModel);
                if (user != null)
                {
                    return new UserModel()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    };
                }
                return null;
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public string LoginUser(LoginModel userLoginDto)
        {
            try
            {
                return _userRl.LoginUser(userLoginDto);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                return await _userRl.ForgetPassword(email);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }

        public bool ResetPassword(string newPassword, string token)
        {
            try
            {
                var principal = _jwtToken.GetTokenValidation(token);
                var userId = Convert.ToInt32(principal.FindFirstValue("UserId"));
                return _userRl.ResetPassword(newPassword, userId);
            }
            catch (RepositoryLayerException ex)
            {
                throw new BusinessLayerException(ex.Message, ex);
            }
        }
    }
}
