using CaseOpener.Core.Models.Item;

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
        /// Case model.
        /// </summary>
        public CasePageModel Case { get; set; } = null!;

        /// <summary>
        /// Item model.
        /// </summary>
        public ItemModel Item { get; set; } = null!;

        /// <summary>
        /// Date of opening.
        /// </summary>
        public DateTime DateOpened { get; set; }
    }
}
