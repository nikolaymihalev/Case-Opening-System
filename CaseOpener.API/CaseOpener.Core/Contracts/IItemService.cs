using CaseOpener.Core.Models.Item;
using CaseOpener.Infrastructure.Models;

namespace CaseOpener.Core.Contracts
{
    public interface IItemService
    {
        Task<IEnumerable<ItemModel>> GetItemsAsync(string adminId);
        Task<ItemModel?> GetItemByIdAsync(int id);
        Task<string> AddItemAsync(ItemFormModel model, string adminId);
        Task<string> EditItemAsync(ItemFormModel model, string adminId);
        Task<string> DeleteItemAsync(int id, string adminId);
        Task<string> AddItemToInventoryAsync(InventoryItem model);
        Task<string> RemoveItemFromInventoryAsync(int id, string userId);
        Task<InventoryItemModel?> GetrInventoryItemByIdAsync(int id, string userId);
    }
}
