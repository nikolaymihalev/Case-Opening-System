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
    public class CategoryServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private CategoryService categoryService;

        [SetUp]
        public void Setup()
        {
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            categoryService = new CategoryService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddAsync_ValidModel_AddsCategoryAndReturnsSuccessMessage()
        {
            var model = new CategoryFormModel
            {
                Name = "TestCategory"
            };

            var result = await categoryService.AddAsync(model);

            var category = await repository.AllReadonly<Category>().FirstOrDefaultAsync(x => x.Name == model.Name);

            Assert.IsNotNull(category);
            Assert.AreEqual(model.Name, category.Name);
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "category"), result);
        }

        [Test]
        public async Task EditAsync_ExistingCategory_UpdatesCategoryAndReturnsSuccessMessage()
        {
            var category = new Category
            {
                Id = 1,
                Name = "OldName"
            };

            await repository.AddAsync(category);
            await repository.SaveChangesAsync();

            var model = new CategoryFormModel
            {
                Id = category.Id,
                Name = "UpdatedName"
            };

            var result = await categoryService.EditAsync(model);

            var updatedCategory = await repository.GetByIdAsync<Category>(category.Id);

            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual(model.Name, updatedCategory.Name);
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyEdited, "category"), result);
        }

        [Test]
        public async Task EditAsync_NonExistingCategory_ThrowsArgumentException()
        {
            var model = new CategoryFormModel
            {
                Id = 999,
                Name = "NonExistingCategory"
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.EditAsync(model));
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCategoriesOrderedByIdDescending()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" },
                new Category { Id = 3, Name = "Category3" }
            };

            foreach(var category in categories)
            {
                await repository.AddAsync(category);
                await repository.SaveChangesAsync();
            }

            var result = await categoryService.GetAllAsync();

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("Category3", result.Last().Name);
            Assert.AreEqual("Category1", result.First().Name);
        }

        [Test]
        public async Task GetByIdAsync_ExistingCategory_ReturnsCategoryModel()
        {
            var category = new Category
            {
                Id = 1,
                Name = "TestCategory"
            };

            await repository.AddAsync(category);
            await repository.SaveChangesAsync();

            var result = await categoryService.GetByIdAsync(category.Id);

            Assert.AreEqual(category.Id, result.Id);
            Assert.AreEqual(category.Name, result.Name);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingCategory_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.GetByIdAsync(999));
        }
    }
}
