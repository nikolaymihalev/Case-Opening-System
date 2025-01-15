using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents mapping between case and user
    /// </summary>
    [Comment("Case User mapping table")]
    public class CaseUser
    {
        /// <summary>
        /// Unique identifier for the case user.
        /// </summary>
        [Key]
        [Comment("Case User identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        [Required]
        [Comment("Case identifier")]
        public int CaseId { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Required]
        [Comment("User identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Quanitity of cases
        /// </summary>
        [Comment("Case quanitity")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(CaseId))]
        public Case Case { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
