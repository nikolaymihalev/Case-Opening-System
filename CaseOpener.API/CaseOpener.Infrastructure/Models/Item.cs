﻿using CaseOpener.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the Item
    /// </summary>
    [Comment("Represents the Item")]
    public class Item
    {
        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        [Key]
        [Comment("Item's indentifier")]
        public int Id { get; set; }

        /// <summary>
        /// Image of the item.
        /// </summary>
        [Required]
        [Comment("Item's image url")]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required]
        [Comment("Item's name")]
        [MinLength(PropertiesConstants.ItemNameMinLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the item.
        /// </summary>
        [Required]
        [Comment("Item's type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Rarity of the item.
        /// </summary>
        [Required]
        [Comment("Item's rarity")]
        public string Rarity { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the item.
        /// </summary>
        [Required]
        [Precision(18, 4)]
        [Comment("Item's amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Collection of case openings.
        /// </summary>
        public ICollection<CaseOpening> CaseOpenings { get; set; } = new List<CaseOpening>();

        /// <summary>
        /// Collection of inventory items.
        /// </summary>
        public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        /// <summary>
        /// Collection of case items.
        /// </summary>
        public ICollection<CaseItem> CaseItems { get; set; } = new List<CaseItem>();
    }
}
