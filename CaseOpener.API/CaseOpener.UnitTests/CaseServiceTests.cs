﻿using CaseOpener.Core.Constants;
using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Services;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Data;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.UnitTests
{
    [TestFixture]
    public class CaseServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private CaseService caseService;

        [SetUp]
        public void Setup()
        {
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            caseService = new CaseService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddCaseAsync_ShouldAddCaseSuccessfully()
        {
            var model = new CaseFormModel
            {
                Name = "Test Case",
                ImageUrl = "http://example.com/image.png",
                Price = 10.0m,
            };

            context.Items.Add(new Item { Id = 1, Name = "Item 1" });
            context.Items.Add(new Item { Id = 2, Name = "Item 2" });
            await context.SaveChangesAsync();

            var result = await caseService.AddCaseAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "case"), result);
            var caseEntity = await context.Cases.FirstOrDefaultAsync(x => x.Name == model.Name);
            Assert.IsNotNull(caseEntity);
            Assert.AreEqual(model.Name, caseEntity.Name);
            Assert.AreEqual(model.ImageUrl, caseEntity.ImageUrl);
            Assert.AreEqual(model.Price, caseEntity.Price);
        }

        [Test]
        public async Task DeleteCaseAsync_ShouldDeleteCaseSuccessfully()
        {
            var caseEntity = new Case { Name = "Test Case", Price = 10.0m, ImageUrl = "http://example.com/image.png" };
            context.Cases.Add(caseEntity);
            await context.SaveChangesAsync();

            var result = await caseService.DeleteCaseAsync(caseEntity.Id);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyDeleted, "case"), result);
            var deletedCase = await context.Cases.FindAsync(caseEntity.Id);
            Assert.IsNull(deletedCase);
        }

        [Test]
        public void DeleteCaseAsync_ShouldThrowException_WhenCaseDoesNotExist()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await caseService.DeleteCaseAsync(999));
            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }
       
        [Test]
        public void GetCaseByIdAsync_ShouldThrowException_WhenCaseDoesNotExist()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await caseService.GetCaseByIdAsync(999));
            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }

        [Test]
        public async Task EditCaseAsync_ShouldEditCaseSuccessfully()
        {
            var caseEntity = new Case { Name = "Test Case", Price = 10.0m, ImageUrl = "http://example.com/image.png" };
            context.Cases.Add(caseEntity);
            await context.SaveChangesAsync();

            var model = new CaseFormModel
            {
                Id = caseEntity.Id,
                Name = "Edited Test Case",
                ImageUrl = "http://example.com/edited_image.png",
                Price = 15.0m,
            };

            var result = await caseService.EditCaseAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyEdited, "case"), result);

            var editedCase = await context.Cases.FindAsync(caseEntity.Id);
            Assert.AreEqual(model.Name, editedCase.Name);
            Assert.AreEqual(model.ImageUrl, editedCase.ImageUrl);
            Assert.AreEqual(model.Price, editedCase.Price);
        }

        [Test]
        public async Task DoesUserHaveCase_ShouldReturnTrue_WhenUserHasCase()
        {
            var userId = "user1";
            var caseEntity = new Case { Name = "Test Case", Price = 10.0m, ImageUrl = "http://example.com/image.png" };
            var item = new Item { Id = 1, Name = caseEntity.Name };
            var inventoryItem = new InventoryItem { UserId = userId, ItemId = item.Id };

            context.Users.Add(new User { Id = userId });
            context.Cases.Add(caseEntity);
            context.Items.Add(item);
            context.InventoryItems.Add(inventoryItem);
            await context.SaveChangesAsync();

            var result = await caseService.DoesUserHaveCase(userId, caseEntity.Id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DoesUserHaveCase_ShouldReturnFalse_WhenUserDoesNotHaveCase()
        {
            var userId = "user1";
            var caseEntity = new Case { Name = "Test Case", Price = 10.0m, ImageUrl = "http://example.com/image.png" };

            context.Users.Add(new User { Id = userId });
            context.Cases.Add(caseEntity);
            await context.SaveChangesAsync();

            var result = await caseService.DoesUserHaveCase(userId, caseEntity.Id);

            Assert.IsFalse(result);
        }
       
        [Test]
        public void OpenCaseAsync_ShouldThrowException_WhenCaseDoesNotExist()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await caseService.OpenCaseAsync(999, "user1"));
            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }

        [Test]
        public void OpenCaseAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await caseService.OpenCaseAsync(1, "invalidUser"));
            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }

        [Test]
        public async Task GetUserOpenedCasesAsync_UserHasOpenedCases_ReturnsListOfCaseOpeningModels()
        {
            // Arrange
            var userId = "test-user-id";

            var category = new Category { Id = 1, Name = "Weapons" };
            var @case = new Case { Id = 1, Name = "Case1", Price = 10.0m, ImageUrl = "image1.jpg", CategoryId = category.Id };
            var item = new Item { Id = 1, Name = "Item1", ImageUrl = "item1.jpg", Amount = 1, Rarity = "Rare", Type = "Weapon" };
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = @case.Id, ItemId = item.Id, DateOpened = DateTime.UtcNow };

            await repository.AddAsync(category);
            await repository.AddAsync(@case);
            await repository.AddAsync(item);
            await repository.AddAsync(caseOpening);
            await repository.SaveChangesAsync();

            // Act
            var result = await caseService.GetUserOpenedCasesAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var opening = result.First();
            Assert.AreEqual(caseOpening.Id, opening.Id);
            Assert.AreEqual(userId, opening.UserId);
            Assert.AreEqual(@case.Id, opening.Case.Id);
            Assert.AreEqual(item.Id, opening.Item.Id);
        }

        [Test]
        public async Task GetUserOpenedCasesAsync_UserHasNoOpenedCases_ReturnsEmptyList()
        {
            // Arrange
            var userId = "test-user-id";

            // Act
            var result = await caseService.GetUserOpenedCasesAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void GetUserOpenedCasesAsync_InvalidCase_ThrowsArgumentException()
        {
            // Arrange
            var userId = "test-user-id";
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = 999, ItemId = 1, DateOpened = DateTime.UtcNow };

            repository.AddAsync(caseOpening).Wait();
            repository.SaveChangesAsync().Wait();

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.GetUserOpenedCasesAsync(userId));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }

        [Test]
        public void GetUserOpenedCasesAsync_InvalidItem_ThrowsArgumentException()
        {
            // Arrange
            var userId = "test-user-id";

            var category = new Category { Id = 1, Name = "Weapons" };
            var @case = new Case { Id = 1, Name = "Case1", Price = 10.0m, ImageUrl = "image1.jpg", CategoryId = category.Id };
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = @case.Id, ItemId = 999, DateOpened = DateTime.UtcNow };

            repository.AddAsync(category).Wait();
            repository.AddAsync(@case).Wait();
            repository.AddAsync(caseOpening).Wait();
            repository.SaveChangesAsync().Wait();

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.GetUserOpenedCasesAsync(userId));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Item"), exception.Message);
        }
    }
}
