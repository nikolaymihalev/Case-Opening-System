using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents case opening
    /// </summary>
    [Comment("Represents case opening")]
    public class CaseOpening
    {
        /// <summary>
        /// Unique identifier for the opening.
        /// </summary>
        [Key]
        [Comment("Opening's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Required]
        [Comment("User's identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        [Required]
        [Comment("Case's identifier")]
        public int CaseId { get; set; }

        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        [Required]
        [Comment("Item's identifier")]
        public int ItemId { get; set; }

        /// <summary>
        /// Date of opening.
        /// </summary>
        [Required]
        [Comment("Opening's open date")]
        public DateTime DateOpened { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(CaseId))]
        public Case Case { get; set; } = null!;

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; } = null!;
    }
}
