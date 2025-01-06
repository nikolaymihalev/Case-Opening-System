using CaseOpener.Core.Constants;
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
                Items = new int[] { 1, 2 }
            };

            // Add some items to the in-memory database
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
                Items = new int[] { 1 }
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
    }
}
