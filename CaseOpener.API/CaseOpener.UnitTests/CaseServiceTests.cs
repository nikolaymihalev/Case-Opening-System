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

            context.Users.Add(new User { Id = userId });
            context.Cases.Add(caseEntity);
            context.CaseUsers.Add(new CaseUser() { Id = 1, CaseId = caseEntity.Id, UserId = userId, Quantity = 1 });
            await context.SaveChangesAsync();

            var result = await caseService.DoesUserHaveCaseAsync(userId, caseEntity.Id);

            Assert.IsTrue(1 == result);
        }

        [Test]
        public async Task DoesUserHaveCase_ShouldReturnFalse_WhenUserDoesNotHaveCase()
        {
            var userId = "user1";
            var caseEntity = new Case { Name = "Test Case", Price = 10.0m, ImageUrl = "http://example.com/image.png" };

            context.Users.Add(new User { Id = userId });
            context.Cases.Add(caseEntity);
            await context.SaveChangesAsync();

            var result = await caseService.DoesUserHaveCaseAsync(userId, caseEntity.Id);

            Assert.IsTrue(0 == result);
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
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = @case.Id, ItemId = item.Id, DateOpened = DateTime.Now };

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
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = 999, ItemId = 1, DateOpened = DateTime.Now };

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
            var caseOpening = new CaseOpening { Id = 1, UserId = userId, CaseId = @case.Id, ItemId = 999, DateOpened = DateTime.Now };

            repository.AddAsync(category).Wait();
            repository.AddAsync(@case).Wait();
            repository.AddAsync(caseOpening).Wait();
            repository.SaveChangesAsync().Wait();

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.GetUserOpenedCasesAsync(userId));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Item"), exception.Message);
        }

        [Test]
        public async Task BuyCaseAsync_ValidData_AddsCaseToUser()
        {
            // Arrange
            var caseId = 1;
            var userId = "test-user-id";
            var quantity = 5;

            var caseEntity = new Case { Id = caseId, Name = "Test Case", Price = 10.0m };
            var userEntity = new User { Id = userId, Username = "TestUser", Balance = 100.0m };

            await repository.AddAsync(caseEntity);
            await repository.AddAsync(userEntity);
            await repository.SaveChangesAsync();

            // Act
            var result = await caseService.BuyCaseAsync(caseId, userId, quantity);

            // Assert
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "case to user"), result);

            var caseUser = await repository.All<CaseUser>()
                .FirstOrDefaultAsync(x => x.CaseId == caseId && x.UserId == userId);

            Assert.IsNotNull(caseUser);
            Assert.AreEqual(quantity, caseUser.Quantity);
        }

        [Test]
        public async Task BuyCaseAsync_ExistingCaseUser_UpdatesQuantity()
        {
            // Arrange
            var caseId = 1;
            var userId = "test-user-id";
            var initialQuantity = 3;
            var additionalQuantity = 2;

            var caseEntity = new Case { Id = caseId, Name = "Test Case", Price = 10.0m };
            var userEntity = new User { Id = userId, Username = "TestUser", Balance = 100.0m };
            var caseUser = new CaseUser { CaseId = caseId, UserId = userId, Quantity = initialQuantity };

            await repository.AddAsync(caseEntity);
            await repository.AddAsync(userEntity);
            await repository.AddAsync(caseUser);
            await repository.SaveChangesAsync();

            // Act
            var result = await caseService.BuyCaseAsync(caseId, userId, additionalQuantity);

            // Assert
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "case to user"), result);

            var updatedCaseUser = await repository.All<CaseUser>()
                .FirstOrDefaultAsync(x => x.CaseId == caseId && x.UserId == userId);

            Assert.IsNotNull(updatedCaseUser);
            Assert.AreEqual(initialQuantity + additionalQuantity, updatedCaseUser.Quantity);
        }

        [Test]
        public void BuyCaseAsync_NonExistentCase_ThrowsArgumentException()
        {
            // Arrange
            var caseId = 999; 
            var userId = "test-user-id";
            var quantity = 1;

            var userEntity = new User { Id = userId, Username = "TestUser", Balance = 100.0m };

            repository.AddAsync(userEntity).Wait();
            repository.SaveChangesAsync().Wait();

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.BuyCaseAsync(caseId, userId, quantity));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Case"), exception.Message);
        }

        [Test]
        public void BuyCaseAsync_NonExistentUser_ThrowsArgumentException()
        {
            // Arrange
            var caseId = 1;
            var userId = "non-existent-user";
            var quantity = 1;

            var caseEntity = new Case { Id = caseId, Name = "Test Case", Price = 10.0m };

            repository.AddAsync(caseEntity).Wait();
            repository.SaveChangesAsync().Wait();

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.BuyCaseAsync(caseId, userId, quantity));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "User"), exception.Message);
        }

        [Test]
        public async Task GetUsersCasesAsync_ValidUserId_ReturnsUserCases()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new User { Id = userId, Username = "TestUser" };
            var case1 = new Case { Id = 1, Name = "Case 1", ImageUrl = "url1", Price = 10.0m };
            var case2 = new Case { Id = 2, Name = "Case 2", ImageUrl = "url2", Price = 20.0m };
            var caseUser1 = new CaseUser { Id = 1, UserId = userId, CaseId = case1.Id, Case = case1 };
            var caseUser2 = new CaseUser { Id = 2, UserId = userId, CaseId = case2.Id, Case = case2 };

            await repository.AddAsync(user);
            await repository.AddAsync(case1);
            await repository.AddAsync(case2);
            await repository.AddAsync(caseUser1);
            await repository.AddAsync(caseUser2);
            await repository.SaveChangesAsync();

            // Act
            var result = await caseService.GetUsersCasesAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            var resultList = result.ToList();
            Assert.AreEqual(case1.Id, resultList[1].Case.Id);
            Assert.AreEqual(case1.Name, resultList[1].Case.Name);
            Assert.AreEqual(case2.Id, resultList[0].Case.Id);
            Assert.AreEqual(case2.Name, resultList[0].Case.Name);
        }

        [Test]
        public void GetUsersCasesAsync_InvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var invalidUserId = "invalid-user-id";

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await caseService.GetUsersCasesAsync(invalidUserId));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "User"), exception.Message);
        }

        [Test]
        public async Task GetUsersCasesAsync_NoCasesForUser_ReturnsEmptyList()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new User { Id = userId, Username = "TestUser" };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            // Act
            var result = await caseService.GetUsersCasesAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
