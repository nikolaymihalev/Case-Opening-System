using CaseOpener.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Items in the case.
        /// </summary>
        [Required]
        [Comment("Case's items")]
        public string Items { get; set; } = string.Empty;

        /// <summary>
        /// Collection of case openings.
        /// </summary>
        public ICollection<CaseOpening> CaseOpenings { get; set; } = new List<CaseOpening>();

        /// <summary>
        /// Collection of daily rewards.
        /// </summary>
        public ICollection<DailyReward> DailyRewards { get; set; } = new List<DailyReward>();
    }
}
