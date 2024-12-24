namespace CaseOpener.Core.Models.User
{
    /// <summary>
    /// Model for user information
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Username chosen by the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Balance of the user.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Date and time the user joined.
        /// </summary>
        public DateTime DateJoined { get; set; }
    }
}
