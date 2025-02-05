using CaseOpener.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the Category for Case
    /// </summary>
    [Comment("Represents the Category")]
    public class Category
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        [Key]
        [Comment("Category's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        [Required]
        [MaxLength(PropertiesConstants.CategoryMaxLength)]
        [Comment("Category's name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of cases.
        /// </summary>
        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}
