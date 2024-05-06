using BusinessLayer.BLException;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using System;
using System.Threading.Tasks;

namespace UserApi.Controllers
{
    /// <summary>
    /// This class is used as Login API Controller
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            _userBL = userBL;
            _logger = logger;
        }


        /// <summary>
        /// This API is used to Register User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Registration(RegistrationModel registrationModel)
        {
            try
            {
                var result = _userBL.RegisterUser(registrationModel);
                var response = new ResponseModel<UserModel>();
                
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "User Registered successfully";
                    response.Data = result;
                    return Created(string.Empty, response);
                }   
                response.Success = false;
                response.Message = "User is already present with same credentials";
                return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {
                _logger.LogError(ex.InnerException, ex.InnerException.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }



        /// <summary>
        /// This API is used for User Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var user = _userBL.LoginUser(loginModel);
                var response = new ResponseModel<string>();
                if (user != null)
                {
                    response.Success = true;
                    response.Message = "User Login successful";
                    response.Data = user;
                    return Ok(response);
                }
                    response.Success = false;
                    response.Message = "User Login failed, Please enter the valid credentials";
                    return Unauthorized(response);
            }
            catch (BusinessLayerException ex)
            {
                _logger.LogError(ex.InnerException,ex.InnerException.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }



        /// <summary>
        /// This API is used to get Forget Password
        /// </summary>
        /// <param name="passwordModel"></param>
        /// <returns></returns>
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel passwordModel)
        {
            try
            {
                var result = await _userBL.ForgetPassword(passwordModel.Email);
                var response = new ResponseModel<string>();
                if (result != null)
                {
                    response.Success = true;
                    response.Message = $"Reset password link sent successfully to your email address {result}";
                    return Ok(response);
                }
                    response.Success = false;
                    response.Message = $"User is not present with email id ={passwordModel.Email}";
                    return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {
                _logger.LogError(ex.InnerException, ex.InnerException.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        /// <summary>
        /// This API is used to Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="resetModel"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult ResetPassword([FromQuery] string token, ResetPasswordModel resetModel)
        {
            try
            {
                var result = _userBL.ResetPassword(resetModel.Password, token);
                var response = new ResponseModel<bool>();
                if (result)
                {
                    response.Success = true;
                    response.Message = "Password reset successful";
                    response.Data = result;
                    return Ok(response);
                }
                    response.Success = false;
                    response.Message = "Error occurred resetting password. Please try again.";
                    return BadRequest(response);
            }
            catch (BusinessLayerException ex)
            {
                _logger.LogError(ex.InnerException, ex.InnerException.Message);
                return StatusCode(500, ex.InnerException.Message);
            }
        }
    }
}

