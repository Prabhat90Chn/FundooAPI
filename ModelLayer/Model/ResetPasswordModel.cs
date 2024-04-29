using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class ResetPasswordModel
    {
        private const string PasswordRegexPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
        private const string PasswordRequiredErrorMessage = "Password Field is Empty";
        private const string PasswordInvalidErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one number, and one special character.";

        [Required(ErrorMessage = PasswordRequiredErrorMessage)]
        [RegularExpression(PasswordRegexPattern, ErrorMessage = PasswordInvalidErrorMessage)]
        public string Password { get; set; }
    }
}
