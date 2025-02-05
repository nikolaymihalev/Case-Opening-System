namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for Case User mapping model
    /// </summary>
    public class CaseUserModel
    {
        /// <summary>
        /// Unique identifier for the case user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Quanitity of cases
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Case model
        /// </summary>
        public CasePageModel? Case { get; set; }
    }
}
