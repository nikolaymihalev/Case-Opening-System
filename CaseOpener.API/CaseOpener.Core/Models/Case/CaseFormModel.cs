using CaseOpener.Core.Constants;
using CaseOpener.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for adding or editing Case
    /// </summary>
    public class CaseFormModel
    {
        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the case.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [StringLength(
            PropertiesConstants.CaseNameMaxLength, 
            MinimumLength = PropertiesConstants.CaseNameMinLength, 
            ErrorMessage = ReturnMessages.StringLength)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Image of the case.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Price of the case.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        public decimal Price { get; set; }

        /// <summary>
        /// Items in the case.
        /// </summary>
        public int[] Items { get; set; } = new int[0];
    }
}
