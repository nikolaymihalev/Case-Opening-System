namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for case opening
    /// </summary>
    public class CaseOpeningModel
    {
        /// <summary>
        /// Unique identifier for the opening.
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
        /// Unique identifier for the item.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Date of opening.
        /// </summary>
        public DateTime DateOpened { get; set; }
    }
}
