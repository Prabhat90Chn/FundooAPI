using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class ForgetPasswordModel
    {
        private const string EmailRegexPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        private const string EmailRequiredErrorMessage = "Email Field is Empty";
        private const string EmailInvalidErrorMessage = "Please enter a valid email address";

        [Required(ErrorMessage = EmailRequiredErrorMessage)]
        [RegularExpression(EmailRegexPattern, ErrorMessage = EmailInvalidErrorMessage)]
        public string Email { get; set; }
    }
}
