using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using RepositoryLayer.JwtToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooApiContext _dbContext;
        private readonly IConfiguration _config;
        private readonly Password_Hash _passwordHash;
        private readonly IEmailSender _emailService;


        public UserRL(FundooApiContext context, IConfiguration config, Password_Hash hash, IEmailSender emailService)
        {
            _dbContext = context;
            _config = config;
            _passwordHash = hash;
            _emailService = emailService;

        }
        public UserEntity RegisterUser(EntityModel userRegistrationDto)
        {
            var existingUser = _dbContext.User.FirstOrDefault<UserEntity>(e => e.Email == userRegistrationDto.Email);
            if (existingUser != null)
            {
                return null; // User already exists
            }

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


        public string LoginUser(LoginModel userLoginDto)
        {
            var validUser = _dbContext.User.FirstOrDefault(e => e.Email == userLoginDto.Email);
            if (validUser != null && _passwordHash.VerifyPassword(userLoginDto.Password, validUser.Password))
            {
                var token = new Jwt_Token(_config);
                return token.GenerateToken(validUser);
            }

            return null; //Invalid Username and Password
        }

        public async Task<string> ForgetPassword(string email)
        {
            var validUser = _dbContext.User.FirstOrDefault(e => e.Email == email);
            if (validUser != null)
            {
                var token = new Jwt_Token(_config);
                var generatedToken = token.GenerateTokenReset(validUser.Email, validUser.UserId);

                var baseUrl = _config["ResetURL:ResetPasswordUrl"];
                var callbackUrl = $"{baseUrl}?token={generatedToken}";

                await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);

                return "Ok";
            }

            return null; // User not found
        }

        public bool ResetPassword(string newPassword, int userId)
        {
            var validUser = _dbContext.User.FirstOrDefault(e => e.UserId == userId);
            if (validUser != null)
            {
                validUser.Password = _passwordHash.PasswordHashing(newPassword);
                _dbContext.SaveChanges();
                return true;
            }

            return false; // User not found
        }
    }
}
