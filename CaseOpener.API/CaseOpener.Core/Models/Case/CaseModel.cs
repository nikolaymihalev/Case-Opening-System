using CaseOpener.Core.Models.Item;

namespace CaseOpener.Core.Models.Case
{
    /// <summary>
    /// Model for information about case
    /// </summary>
    public class CaseModel
    {
        /// <summary>
        /// Unique identifier for the case.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the case.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Image of the case.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Price of the case.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Items in the case.
        /// </summary>
        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
    }
}
