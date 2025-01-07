using CaseOpener.Core.Constants;
using CaseOpener.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for adding or editing Category
    /// </summary>
    public class CategoryFormModel
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [Required(ErrorMessage = ReturnMessages.Required)]
        [StringLength(PropertiesConstants.CategoryMaxLength, 
            MinimumLength = PropertiesConstants.CategoryMinLength,
            ErrorMessage = ReturnMessages.StringLength)]
        public string Name { get; set; } = string.Empty;
    }
}
