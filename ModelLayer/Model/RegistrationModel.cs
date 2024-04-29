using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{

    public class RegistrationModel
    {
        private const string FirstNameRegex = @"^[a-zA-Z]{2,}$";
        private const string LastNameRegex = @"^[a-zA-Z]{2,}$";
        private const string EmailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private const string PasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

        private const string FirstNameRequiredErrorMessage = "First Name field is empty";
        private const string FirstNameInvalidErrorMessage = "Enter a valid First Name";
        private const string LastNameRequiredErrorMessage = "Last Name field is empty";
        private const string LastNameInvalidErrorMessage = "Enter a valid Last Name";
        private const string EmailRequiredErrorMessage = "Email field is empty";
        private const string EmailInvalidErrorMessage = "Please enter a valid email address";
        private const string PasswordRequiredErrorMessage = "Password field is empty";
        private const string PasswordInvalidErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one number, and one special character.";

        [Required(ErrorMessage = FirstNameRequiredErrorMessage)]
        [RegularExpression(FirstNameRegex, ErrorMessage = FirstNameInvalidErrorMessage)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequiredErrorMessage)]
        [RegularExpression(LastNameRegex, ErrorMessage = LastNameInvalidErrorMessage)]
        public string LastName { get; set; }

        [Required(ErrorMessage = EmailRequiredErrorMessage)]
        [RegularExpression(EmailRegex, ErrorMessage = EmailInvalidErrorMessage)]
        public string Email { get; set; }

        [Required(ErrorMessage = PasswordRequiredErrorMessage)]
        [RegularExpression(PasswordRegex, ErrorMessage = PasswordInvalidErrorMessage)]
        public string Password { get; set; }
    }
}