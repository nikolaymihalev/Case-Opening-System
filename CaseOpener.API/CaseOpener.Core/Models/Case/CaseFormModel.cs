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
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        [StringLength(
            PropertiesConstants.CASE_NAME_MAX_LENGTH, 
            MinimumLength = PropertiesConstants.CASE_NAME_MIN_LENGTH, 
            ErrorMessage = ReturnMessages.STRING_LENGTH)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Image of the case.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public byte[] Image { get; set; } = new byte[128];

        /// <summary>
        /// Price of the case.
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.REQUIRED)]
        public decimal Price { get; set; }

        /// <summary>
        /// Items in the case.
        /// </summary>
        public int[] Items { get; set; } = new int[0];
    }
}
