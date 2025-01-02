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
        private readonly IAdminService adminService;

        public ItemService(
            IRepository _repository,
            IAdminService _adminService)
        {
            repository = _repository;
            adminService = _adminService;
        }

        public async Task<string> AddItemAsync(ItemFormModel model, string adminId)
        {
            if(await adminService.CheckUserIsAdmin(adminId))
            {
                if (IsValidEnumValue<ItemRarity>(model.Rarity) == false || IsValidEnumValue<ItemRarity>(model.Type))
                    return ReturnMessages.InvalidModel;

                var item = new Item()
                {
                    Name = model.Name,
                    Type = model.Type,
                    Rarity = model.Rarity,
                    Probability = model.Probability,
                    ImageUrl = model.ImageUrl,
                    Amount = model.Amount,
                };

                await repository.AddAsync(item);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyAdded, "item");
            }

            return ReturnMessages.Unauthorized;
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

        public async Task<string> DeleteItemAsync(int id, string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                var item = await repository.GetByIdAsync<Item>(id);

                if (item is null)
                    return string.Format(ReturnMessages.DoesntExist, "item");

                await repository.DeleteAsync<Item>(id);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyDeleted, "item");
            }

            return ReturnMessages.Unauthorized;
        }

        public async Task<string> EditItemAsync(ItemFormModel model, string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                var item = await repository.GetByIdAsync<Item>(model.Id);

                if (item is null)
                    return string.Format(ReturnMessages.DoesntExist, "item");

                if (IsValidEnumValue<ItemRarity>(model.Rarity) == false || IsValidEnumValue<ItemRarity>(model.Type))
                    return ReturnMessages.InvalidModel;

                item.Amount = model.Amount;
                item.Rarity = model.Rarity;
                item.Name = model.Name;
                item.Type = model.Type;
                item.ImageUrl = model.ImageUrl;
                item.Probability = model.Probability;

                await repository.SaveChangesAsync();
            }

            return ReturnMessages.Unauthorized;
        }

        public async Task<ItemModel?> GetItemByIdAsync(int id)
        {
            var item = await repository.GetByIdAsync<Item>(id);

            if(item is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "item"));

            return new ItemModel()
            {
                Id = id,
                Name = item.Name,
                Rarity = item.Rarity,
                Type = item.Type,
                Amount = item.Amount,
                Probability = item.Probability,
                ImageUrl = item.ImageUrl
            };
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                return await repository.AllReadonly<Item>()
                    .Select(x => new ItemModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Rarity = x.Rarity,
                        Type = x.Type,
                        Amount = x.Amount,
                        Probability = x.Probability,
                        ImageUrl = x.ImageUrl
                    }).ToListAsync();
            }

            throw new ArgumentException(ReturnMessages.Unauthorized);
        }

        public async Task<InventoryItemModel?> GetrInventoryItemByIdAsync(int id, string userId)
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
                return ReturnMessages.Unauthorized;

            await repository.DeleteAsync<InventoryItem>(id);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyDeleted, "inventory item");
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
