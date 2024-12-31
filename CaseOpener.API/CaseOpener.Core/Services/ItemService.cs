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
                    return ReturnMessages.INVALID_MODEL;

                var item = new Item()
                {
                    Name = model.Name,
                    Type = model.Type,
                    Rarity = model.Rarity,
                    Probability = model.Probability,
                    Image = model.Image,
                    Amount = model.Amount,
                };

                await repository.AddAsync(item);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SUCCESSFYLLY_ADDED, "item");
            }

            return ReturnMessages.UNAUTHORIZED;
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

            return string.Format(ReturnMessages.SUCCESSFYLLY_ADDED, "item to inventory");
        }

        public async Task<string> DeleteItemAsync(int id, string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                var item = await repository.GetByIdAsync<Item>(id);

                if (item is null)
                    return string.Format(ReturnMessages.DOESNT_EXIST, "item");

                await repository.DeleteAsync<Item>(id);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SUCCESSFULLY_DELETED, "item");
            }

            return ReturnMessages.UNAUTHORIZED;
        }

        public async Task<string> EditItemAsync(ItemFormModel model, string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                var item = await repository.GetByIdAsync<Item>(model.Id);

                if (item is null)
                    return string.Format(ReturnMessages.DOESNT_EXIST, "item");

                if (IsValidEnumValue<ItemRarity>(model.Rarity) == false || IsValidEnumValue<ItemRarity>(model.Type))
                    return ReturnMessages.INVALID_MODEL;

                item.Amount = model.Amount;
                item.Rarity = model.Rarity;
                item.Name = model.Name;
                item.Type = model.Type;
                item.Image = model.Image;
                item.Probability = model.Probability;

                await repository.SaveChangesAsync();
            }

            return ReturnMessages.UNAUTHORIZED;
        }

        public async Task<ItemModel?> GetItemByIdAsync(int id)
        {
            var item = await repository.GetByIdAsync<Item>(id);

            if(item is null)
                throw new ArgumentException(string.Format(ReturnMessages.DOESNT_EXIST, "item"));

            return new ItemModel()
            {
                Id = id,
                Name = item.Name,
                Rarity = item.Rarity,
                Type = item.Type,
                Amount = item.Amount,
                Probability = item.Probability,
                Image = Convert.ToBase64String(item.Image)
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
                        Image = Convert.ToBase64String(x.Image)
                    }).ToListAsync();
            }

            throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
        }

        public async Task<InventoryItemModel?> GetrInventoryItemByIdAsync(int id, string userId)
        {
            var inventoryItem = await repository.GetByIdAsync<InventoryItem>(id);

            if (inventoryItem is null)
                throw new ArgumentException(string.Format(ReturnMessages.DOESNT_EXIST, "Inventory item"));

            if (inventoryItem.UserId != userId)
                throw new ArgumentException(ReturnMessages.UNAUTHORIZED);

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
                throw new ArgumentException(string.Format(ReturnMessages.DOESNT_EXIST, "Inventory item"));

            if (inventoryItem.UserId != userId)
                return ReturnMessages.UNAUTHORIZED;

            await repository.DeleteAsync<InventoryItem>(id);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SUCCESSFULLY_DELETED, "inventory item");
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
