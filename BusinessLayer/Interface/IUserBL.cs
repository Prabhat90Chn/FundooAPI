using ModelLayer.Model;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserModel RegisterUser(RegistrationModel userRegistration);
        public string LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, string token);
    }
}
