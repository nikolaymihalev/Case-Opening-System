namespace CaseOpener.Core.Models.Item
{
    /// <summary>
    /// Model for information about item
    /// </summary>
    public class ItemModel
    {
        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Image of the item.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Name of the item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the item.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Rarity of the item.
        /// </summary>
        public string Rarity { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the item.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Probability chance of the item.
        /// </summary>
        public double Probability { get; set; }
    }
}
