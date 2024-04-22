using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Model;
using Newtonsoft.Json.Linq;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooAPI.Controllers
{
    /// <summary>
    /// This class is used as API Controller
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class FundooApiController : ControllerBase
    {
        private  readonly IUserBL _userBL;
        private readonly IConfiguration _config;
        public FundooApiController(IUserBL userBL, IConfiguration config)
        {  
            _userBL = userBL;
            _config = config;
        }

        /// <summary>
        /// This API is used to Register User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registration")]
        public ResponseModel<EntityModel> Registration(EntityModel userModel)
        {
            var response = new ResponseModel<EntityModel>();
            var result = _userBL.RegisterUser(userModel);
            if (result != null)
            {
                response.Success = true;
                response.Message = "User Registered successfully";
                response.Data = userModel;
            }
            else
            {
                response.Success = false;
                response.Message = "User Registeration failed";
            }
            return response;
        }


        /// <summary>
        /// This API is used for User Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ResponseModel<string> Login(LoginModel loginModel)
        {
            var response = new ResponseModel<string>();
            var token = _userBL.LoginUser(loginModel);
            if (token != null)
            {
                response.Success = true;
                response.Message = "User Login successful";
                response.Data = token;
            }
            else
            {
                response.Success = false;
                response.Message = "User Login failed, Please enter the valid credentials";
            }
            return response;
        }


        /// <summary>
        /// his API is used to get Forget Password
        /// </summary>
        /// <param name="passwordModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel passwordModel)
        {
            var response = new ResponseModel<string>();
            var result = await _userBL.ForgetPassword(passwordModel.Email);
            if (result != null)
            {
                response.Success = true;
                response.Message = $"Reset password link sent successfully to your email address {result}";
            }
            else
            {
                response.Success = false;
                response.Message = "Unexpected error occurred, please try again";
            }
            return Ok(response);
        }

        /// <summary>
        /// This API is used to Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="resetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword([FromQuery] string token, ResetPasswordModel resetModel)
        {
            var response = new ResponseModel<bool>();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidAudience = _config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(token, validationParameters, out validatedToken);
                var userId = principal.FindFirstValue("UserId");
                int _userId = Convert.ToInt32(userId);

                var result = _userBL.ResetPassword(resetModel.Password, _userId);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Password reset successful";
                    response.Data = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = "An unexpected error occurred. Please try again.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error resetting password: {ex.Message}";
            }
            return Ok(response);
        }
    }
}

