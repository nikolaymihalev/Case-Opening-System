using CaseOpener.Core.Models.Item;

namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for information about case
    /// </summary>
    public class CaseModel
    {
        /// <summary>
        /// Case model
        /// </summary>
        public CasePageModel Case { get; set; } = null!;

        /// <summary>
        /// Items in the case.
        /// </summary>
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
    }
}
