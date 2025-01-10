using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Enums;
using CaseOpener.Core.Models.Item;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IRepository repository;

        public ItemService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<string> AddItemAsync(ItemFormModel model)
        {
            if (IsValidEnumValue<ItemRarity>(model.Rarity) == false || IsValidEnumValue<ItemRarity>(model.Type))
                throw new ArgumentException(ReturnMessages.InvalidModel);

            var item = new Item()
            {
                Name = model.Name,
                Type = model.Type,
                Rarity = model.Rarity,
                ImageUrl = model.ImageUrl,
                Amount = model.Amount,
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "item");
         }

        public async Task<string> AddItemToInventoryAsync(InventoryItem model)
        {
            var inventoryItem = new InventoryItem()
            {
                UserId = model.UserId,
                ItemId = model.ItemId,
                AcquiredDate = DateTime.UtcNow,
            };

            await repository.AddAsync(inventoryItem);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "item to inventory");
        }

        public async Task<string> DeleteItemAsync(int id)
        {
            var item = await repository.GetByIdAsync<Item>(id);

            if (item is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

            await repository.DeleteAsync<Item>(id);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyDeleted, "item");
        }

        public async Task<string> EditItemAsync(ItemFormModel model)
        {
            var item = await repository.GetByIdAsync<Item>(model.Id);

            if (item is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

            if (IsValidEnumValue<ItemRarity>(model.Rarity) == false || IsValidEnumValue<ItemRarity>(model.Type))
                throw new ArgumentException(ReturnMessages.InvalidModel);

            item.Amount = model.Amount;
            item.Rarity = model.Rarity;
            item.Name = model.Name;
            item.Type = model.Type;
            item.ImageUrl = model.ImageUrl;

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyEdited, "item");
        }

        public async Task<ItemModel> GetItemByIdAsync(int id)
        {
            var item = await repository.GetByIdAsync<Item>(id);

            if(item is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Item"));

            return new ItemModel()
            {
                Id = id,
                Name = item.Name,
                Rarity = item.Rarity,
                Type = item.Type,
                Amount = item.Amount,
                ImageUrl = item.ImageUrl
            };
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync()
        {
            return await repository.AllReadonly<Item>()
                    .Select(x => new ItemModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Rarity = x.Rarity,
                        Type = x.Type,
                        Amount = x.Amount,
                        ImageUrl = x.ImageUrl
                    })
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
        }

        public async Task<InventoryItemModel> GetInventoryItemByIdAsync(int id, string userId)
        {
            var inventoryItem = await repository.GetByIdAsync<InventoryItem>(id);

            if (inventoryItem is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Inventory item"));

            if (inventoryItem.UserId != userId)
                throw new ArgumentException(ReturnMessages.Unauthorized);

            return new InventoryItemModel()
            {
                Id = id,
                UserId = userId,
                ItemId = inventoryItem.ItemId,
                AcquiredDate = inventoryItem.AcquiredDate,
            };
        }

        public async Task<string> RemoveItemFromInventoryAsync(int id, string userId)
        {
            var inventoryItem = await repository.GetByIdAsync<InventoryItem>(id);

            if (inventoryItem is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Inventory item"));

            if (inventoryItem.UserId != userId)
                throw new ArgumentException(ReturnMessages.Unauthorized);

            await repository.DeleteAsync<InventoryItem>(id);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyDeleted, "inventory item");
        }
        public async Task<IEnumerable<ItemModel>> GetUserInventoryItemsAsync(string userId)
        {
            var inItems = await repository.AllReadonly<InventoryItem>().Where(x => x.UserId == userId).ToListAsync();

            var items = new List<Item>();

            for (int i = 0; i < inItems.Count(); i++)
            {
                var item = await repository.GetByIdAsync<Item>(inItems[i].ItemId);

                if (item != null)
                    items.Add(item);
            }

            return items.Select(x=>new ItemModel()
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Type = x.Type,
                Rarity = x.Rarity,
                Amount = x.Amount,
            });
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
