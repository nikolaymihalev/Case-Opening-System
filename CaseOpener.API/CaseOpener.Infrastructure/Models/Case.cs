using CaseOpener.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the Case
    /// </summary>
    [Comment("Represents the Case")]
    public class Case
    {
        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        [Key]
        [Comment("Case's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the case.
        /// </summary>
        [Required]
        [MinLength(PropertiesConstants.CaseNameMinLength)]
        [Comment("Case's name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        [Required]
        [Comment("Category's identifier")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Image of the case.
        /// </summary>
        [Required]
        [Comment("Case's image")]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Price of the case.
        /// </summary>
        [Required]
        [Precision(18, 4)]
        [Comment("Case's price")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Collection of case openings.
        /// </summary>
        public ICollection<CaseOpening> CaseOpenings { get; set; } = new List<CaseOpening>();

        /// <summary>
        /// Collection of case items.
        /// </summary>
        public ICollection<CaseItem> CaseItems { get; set; } = new List<CaseItem>();

        /// <summary>
        /// Collection of case users.
        /// </summary>
        public ICollection<CaseUser> CaseUsers { get; set; } = new List<CaseUser>();
    }
}
