using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the user's inventory item
    /// </summary>
    [Comment("Represents the user's inventory item")]
    public class InventoryItem
    {
        /// <summary>
        /// Unique identifier for the inventory item.
        /// </summary>
        [Key]
        [Comment("Inventory item's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Required]
        [Comment("User's identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        [Required]
        [Comment("Item's identifier")]
        public int ItemId { get; set; }

        /// <summary>
        /// Date of acquisition of the inventory item
        /// </summary>
        [Required]
        [Comment("Inventory item's acquired date")]
        public DateTime AcquiredDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; } = null!;
    }
}
