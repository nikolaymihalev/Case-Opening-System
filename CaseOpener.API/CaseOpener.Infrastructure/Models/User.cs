using CaseOpener.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the User
    /// </summary>
    [Comment("Represents the User")]
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Key]
        [Comment("User's identifier")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Username chosen by the user.
        /// </summary>
        [Required]
        [Comment("User's username")]
        [MinLength(PropertiesConstants.UsernameMinLength)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user.
        /// </summary>
        [Required]
        [Comment("User's email")]
        [MinLength(PropertiesConstants.EmailMinLength)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hashed password for secure authentication.
        /// </summary>
        [Required]
        [Comment("User's password")]
        [MinLength(PropertiesConstants.PasswordMinLength)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Balance of the user.
        /// </summary>
        [Required]
        [Precision(18, 4)]
        [Comment("User's balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// Date and time the user joined.
        /// </summary>
        [Required]
        [Comment("The date when user joined")]
        public DateTime DateJoined { get; set; }

        /// <summary>
        /// Transactions created by the user.
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Case openings created by the user.
        /// </summary>
        public ICollection<CaseOpening> CaseOpenings { get; set; } = new List<CaseOpening>();

        /// <summary>
        /// Inventory items added by the user.
        /// </summary>
        public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        /// <summary>
        /// Daily rewards for user.
        /// </summary>
        public ICollection<DailyReward> DailyRewards { get; set; } = new List<DailyReward>();

        /// <summary>
        /// Roles for user.
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        /// <summary>
        /// Collection of case users.
        /// </summary>
        public ICollection<CaseUser> CaseUsers { get; set; } = new List<CaseUser>();
    }
}
