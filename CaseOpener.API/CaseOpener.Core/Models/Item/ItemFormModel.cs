using CaseOpener.Core.Constants;
using CaseOpener.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Core.Models.Item
{
    /// <summary>
    /// Model for adding or editing Item
    /// </summary>
    public class ItemFormModel
    {
        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Image of the item.
        /// </summary>
        public byte[] Image { get; set; } = new byte[128];

        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        [StringLength(
            PropertiesConstants.ITEM_NAME_MAX_LENGTH,
            MinimumLength = PropertiesConstants.ITEM_NAME_MIN_LENGTH,
            ErrorMessage = ReturnMessages.STRING_LENGTH)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Rarity of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public string Rarity { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Probability chance of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public double Probability { get; set; }
    }
}
