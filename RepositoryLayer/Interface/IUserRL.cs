using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity RegisterUser(RegistrationModel userRegistration);
        public string LoginUser(LoginModel userLoginDto);
        public Task<string> ForgetPassword(string email);
        public bool ResetPassword(string newPassword, int userId);

    }
}
