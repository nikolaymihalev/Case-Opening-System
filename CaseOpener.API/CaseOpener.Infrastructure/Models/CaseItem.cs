using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents mapping between case and item
    /// </summary>
    [Comment("Represents mapping between case and item")]
    public class CaseItem
    { 
        /// <summary>
        /// Unique identifier for the opening.
        /// </summary>
        [Key]
        [Comment("Case item identifier")]
        public int Id { get; set; }

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
        /// Probability chance of the item.
        /// </summary>
        [Required]
        [Comment("Item's probability chance")]
        public double Probability { get; set; }

        [ForeignKey(nameof(CaseId))]
        public Case Case { get; set; } = null!;

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; } = null!;
    }
}
