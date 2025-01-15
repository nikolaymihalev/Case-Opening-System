using CaseOpener.Core.Models.Item;

namespace CaseOpener.Core.Contracts
{
    public interface IItemService
    {
        Task<IEnumerable<ItemModel>> GetItemsAsync();
        Task<ItemModel> GetItemByIdAsync(int id);
        Task<string> AddItemAsync(ItemFormModel model);
        Task<string> EditItemAsync(ItemFormModel model);
        Task<string> DeleteItemAsync(int id);
        Task<string> AddItemToInventoryAsync(InventoryItemModel model);
        Task<string> RemoveItemFromInventoryAsync(int id, string userId);
        Task<InventoryItemModel> GetInventoryItemByIdAsync(int id, string userId);
        Task<IEnumerable<ItemModel>> GetUserInventoryItemsAsync(string userId);        
    }
}
