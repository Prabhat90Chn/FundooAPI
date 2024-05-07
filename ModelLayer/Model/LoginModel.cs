using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class LoginModel
    {
        private const string EmailRegexPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private const string EmailRequiredErrorMessage = "Email Field is Empty";
        private const string EmailInvalidErrorMessage = "Please enter a valid email address";

        private const string PasswordRegexPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
        private const string PasswordRequiredErrorMessage = "Password Field is Empty";
        private const string PasswordInvalidErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one number, and one special character.";

        //[Required(ErrorMessage = EmailRequiredErrorMessage)]
        [RegularExpression(EmailRegexPattern, ErrorMessage = EmailInvalidErrorMessage)]
        public string Email { get; set; }

        // [Required(ErrorMessage = PasswordRequiredErrorMessage)]
        [RegularExpression(PasswordRegexPattern, ErrorMessage = PasswordInvalidErrorMessage)]
        public string Password { get; set; }
    }
}
