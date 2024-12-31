namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for daily reward
    /// </summary>
    public class DailyRewardModel
    {
        /// <summary>
        /// Unique identifier for the reward.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// The last claimed date of the daily reward.
        /// </summary>
        public DateTime LastClaimedDate { get; set; }
    }
}
