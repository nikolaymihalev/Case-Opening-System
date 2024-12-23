using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the Transaction
    /// </summary>
    [Comment("Represents the Transaction")]
    public class Transaction
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        [Key]
        [Comment("Transaction's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        [Required]
        [Comment("User's identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Type of the transaction.
        /// </summary>
        [Required]
        [Comment("Transaction's type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the transaction.
        /// </summary>
        [Required]
        [Comment("Transaction's amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of the transaction.
        /// </summary>
        [Required]
        [Comment("Transaction's date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Status of the transaction.
        /// </summary>
        [Required]
        [Comment("Transaction's status")]
        public string Status { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
