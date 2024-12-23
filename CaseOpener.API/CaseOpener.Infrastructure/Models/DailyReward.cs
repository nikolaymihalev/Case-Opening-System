using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CaseOpener.Infrastructure.Models
{
    /// <summary>
    /// Represents the user's daily reward
    /// </summary>
    [Comment("Represents the user's daily reward")]
    public class DailyReward
    {
        /// <summary>
        /// Unique identifier for the reward.
        /// </summary>
        [Key]
        [Comment("Reward's identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Required]
        [Comment("User's identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        [Required]
        [Comment("Case's identifier")]
        public int CaseId { get; set; }

        /// <summary>
        /// The last claimed date of the daily reward.
        /// </summary>
        [Required]
        [Comment("Case's claimed date")]
        public DateTime LastClaimedDate { get; set; }
    }
}
