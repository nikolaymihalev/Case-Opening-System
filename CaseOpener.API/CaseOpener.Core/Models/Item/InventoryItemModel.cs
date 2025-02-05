namespace CaseOpener.Core.Models.Item
{
    /// <summary>
    /// Model for inventory item
    /// </summary>
    public class InventoryItemModel
    {
        /// <summary>
        /// Unique identifier for the inventory item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Date of acquisition of the inventory item
        /// </summary>
        public DateTime AcquiredDate { get; set; }
    }
}
