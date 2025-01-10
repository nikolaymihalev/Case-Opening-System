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
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [StringLength(
            PropertiesConstants.ItemNameMaxLength,
            MinimumLength = PropertiesConstants.ItemNameMinLength,
            ErrorMessage = ReturnMessages.StringLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Rarity of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        public string Rarity { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the item.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        public decimal Amount { get; set; }
    }
}
