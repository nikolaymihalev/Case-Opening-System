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
        [Comment("Item's image")]
        public byte[] Image { get; set; } = new byte[128];

        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required]
        [Comment("Item's name")]
        [MinLength(PropertiesConstants.ITEM_NAME_MIN_LENGTH)]
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
        [Comment("Item's amount")]
        public decimal Amount { get; set; }
    }
}
