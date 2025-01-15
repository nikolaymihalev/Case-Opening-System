using CaseOpener.Core.Constants;
using CaseOpener.Core.Models.Item;
using CaseOpener.Core.Services;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Data;
using CaseOpener.Infrastructure.Models;

namespace CaseOpener.UnitTests
{
    [TestFixture]
    public class ItemServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private ItemService itemService;

        [SetUp]
        public void Setup()
        {
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            itemService = new ItemService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddItemAsync_ValidItem_ReturnsSuccessMessage()
        {
            var model = new ItemFormModel
            {
                Name = "Test Item",
                Type = "Skin",
                Rarity = "Industrial",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            var result = await itemService.AddItemAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "item"), result);
        }

        [Test]
        public async Task AddItemAsync_InvalidRarity_ThrowsArgumentException()
        {
            var model = new ItemFormModel
            {
                Name = "Test Item",
                Type = "Weapon",
                Rarity = "InvalidRarity",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await itemService.AddItemAsync(model));
        }

        [Test]
        public async Task DeleteItemAsync_ExistingItem_ReturnsSuccessMessage()
        {
            var item = new Item
            {
                Name = "Test Item",
                Type = "Weapon",
                Rarity = "Industrial",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            var result = await itemService.DeleteItemAsync(item.Id);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyDeleted, "item"), result);
        }

        [Test]
        public async Task DeleteItemAsync_NonExistingItem_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await itemService.DeleteItemAsync(999)); 
        }

        [Test]
        public async Task EditItemAsync_ValidItem_ReturnsSuccessMessage()
        {
            var item = new Item
            {
                Name = "Test Item",
                Type = "Skin",
                Rarity = "Industrial",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            var model = new ItemFormModel
            {
                Id = item.Id,
                Name = "Updated Item",
                Type = "Gloves",
                Rarity = "Covert",
                ImageUrl = "http://example.com/updated-item.png",
                Amount = 10
            };

            var result = await itemService.EditItemAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyEdited, "item"), result);
        }

        [Test]
        public async Task EditItemAsync_NonExistingItem_ThrowsArgumentException()
        {
            var model = new ItemFormModel
            {
                Id = 999,
                Name = "Updated Item",
                Type = "Armor",
                Rarity = "Rare",
                ImageUrl = "http://example.com/updated-item.png",
                Amount = 10
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await itemService.EditItemAsync(model));
        }

        [Test]
        public async Task GetItemByIdAsync_ExistingItem_ReturnsItemModel()
        {
            var item = new Item
            {
                Name = "Test Item",
                Type = "Weapon",
                Rarity = "Epic",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            var result = await itemService.GetItemByIdAsync(item.Id);

            Assert.AreEqual(item.Id, result.Id);
            Assert.AreEqual(item.Name, result.Name);
        }

        [Test]
        public async Task GetItemByIdAsync_NonExistingItem_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await itemService.GetItemByIdAsync(999));
        }

        [Test]
        public async Task GetItemsAsync_ReturnsItemsList()
        {
            var item1 = new Item
            {
                Name = "Test Item 1",
                Type = "Weapon",
                Rarity = "Epic",
                ImageUrl = "http://example.com/item1.png",
                Amount = 5
            };

            var item2 = new Item
            {
                Name = "Test Item 2",
                Type = "Armor",
                Rarity = "Rare",
                ImageUrl = "http://example.com/item2.png",
                Amount = 10
            };

            await repository.AddAsync(item1);
            await repository.AddAsync(item2);
            await repository.SaveChangesAsync();

            var result = await itemService.GetItemsAsync();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(item1.Name, result.Last().Name);
            Assert.AreEqual(item2.Name, result.First().Name);
        }

        [Test]
        public async Task AddItemToInventoryAsync_ValidItem_ReturnsSuccessMessage()
        {
            var item = new Item
            {
                Name = "Test Item",
                Type = "Weapon",
                Rarity = "Epic",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            var inventoryItem = new InventoryItemModel
            {
                UserId = "user123",
                ItemId = item.Id
            };

            var result = await itemService.AddItemToInventoryAsync(inventoryItem);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "item to inventory"), result);
        }

        [Test]
        public async Task RemoveItemFromInventoryAsync_ValidItem_ReturnsSuccessMessage()
        {
            var item = new Item
            {
                Name = "Test Item",
                Type = "Weapon",
                Rarity = "Epic",
                ImageUrl = "http://example.com/item.png",
                Amount = 5
            };

            await repository.AddAsync(item);
            await repository.SaveChangesAsync();

            var inventoryItem = new InventoryItem
            {
                UserId = "user123",
                ItemId = item.Id
            };

            await repository.AddAsync(inventoryItem);
            await repository.SaveChangesAsync();

            var result = await itemService.RemoveItemFromInventoryAsync(inventoryItem.Id, inventoryItem.UserId);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyDeleted, "inventory item"), result);
        }

        [Test]
        public async Task RemoveItemFromInventoryAsync_NonExistingInventoryItem_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await itemService.RemoveItemFromInventoryAsync(999, "user123")); 
        }
    }

}
