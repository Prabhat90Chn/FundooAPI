using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using RepositoryLayer.RLException;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooApiContext _dbContext;
        private readonly IConfiguration _config;
        private readonly Password_Hash _passwordHash;
        private readonly IEmailSender _emailService;
        private readonly ILogger<UserRL> _logger;


        public UserRL(FundooApiContext context, IConfiguration config, Password_Hash hash, IEmailSender emailService, ILogger<UserRL> logger)
        {
            _dbContext = context;
            _config = config;
            _passwordHash = hash;
            _emailService = emailService;
            _logger = logger;
        }
        public UserEntity RegisterUser(RegistrationModel userRegistrationDto)
        {
            try
            {
                var existingUser = _dbContext.User.FirstOrDefault<UserEntity>(e => e.Email == userRegistrationDto.Email);

                if (existingUser == null)
                {
                    var newUser = new UserEntity
                    {
                        FirstName = userRegistrationDto.FirstName,
                        LastName = userRegistrationDto.LastName,
                        Email = userRegistrationDto.Email,
                        Password = _passwordHash.PasswordHashing(userRegistrationDto.Password)
                    };

                    _dbContext.User.Add(newUser);
                    _dbContext.SaveChanges();

                    return newUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in RegisterUser method.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }


        public string LoginUser(LoginModel userLoginDto)
        {
            try
            {
                var validUser = _dbContext.User.FirstOrDefault(e => e.Email == userLoginDto.Email);

                if (validUser != null && _passwordHash.VerifyPassword(userLoginDto.Password, validUser.Password))
                {
                    var token = new JwtToken.JwtToken(_config);
                    return token.GenerateToken(validUser);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in LoginUser method.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }

        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var validUser = _dbContext.User.FirstOrDefault(e => e.Email == email);

                if (validUser != null)
                {
                    var token = new JwtToken.JwtToken(_config);
                    var generatedToken = token.GenerateTokenReset(validUser.Email, validUser.UserId);

                    var baseUrl = _config["ResetURL:ResetPasswordUrl"];
                    var callbackUrl = $"{baseUrl}?token={generatedToken}";

                    await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);

                    return "Ok";
                }
                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in ForgetPassword method.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }


        public bool ResetPassword(string newPassword, int userId)
        {
            try
            {
                var validUser = _dbContext.User.FirstOrDefault(e => e.UserId == userId);

                if (validUser != null)
                {
                    validUser.Password = _passwordHash.PasswordHashing(newPassword);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in ResetPassword method.");
                throw new RepositoryLayerException(ex.Message, ex);
            }
        }
    }
}
