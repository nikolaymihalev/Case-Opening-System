using CaseOpener.Core.Constants;
using CaseOpener.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Core.Models.User
{
    /// <summary>
    /// Model for user registration.
    /// Contains the user's username, email, first and last name, password, and a confirmation password field 
    /// required during the account registration process.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// The username that the user will register with
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [StringLength(PropertiesConstants.UsernameMaxLength,
            MinimumLength = PropertiesConstants.UsernameMinLength,
            ErrorMessage = ReturnMessages.StringLength)]
        public string Username { get; set; } = null!;

        /// <summary>
        /// The user's email address used for account registration
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [EmailAddress]
        [StringLength(PropertiesConstants.EmailMaxLength,
            MinimumLength = PropertiesConstants.EmailMinLength,
            ErrorMessage = ReturnMessages.StringLength)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password used for account registration and future authentication
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// A field to confirm the user's password, ensuring the password is entered
        /// </summary>
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
