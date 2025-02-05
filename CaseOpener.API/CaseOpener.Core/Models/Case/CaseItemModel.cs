namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for case item mapping table
    /// </summary>
    public class CaseItemModel
    {
        /// <summary>
        /// Unique identifier for the opening.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int CaseId { get; set; }

        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Probability chance of the item.
        /// </summary>
        public double Probability { get; set; }
    }
}
