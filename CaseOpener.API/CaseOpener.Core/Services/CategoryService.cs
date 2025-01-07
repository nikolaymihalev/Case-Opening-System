using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;

        public CategoryService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<string> AddAsync(CategoryFormModel model)
        {
            var category = new Category()
            {
                Name = model.Name
            };

            await repository.AddAsync(category);
            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyAdded, "category");
        }

        public async Task<string> EditAsync(CategoryFormModel model)
        {
            var category = await repository.GetByIdAsync<Category>(model.Id);

            if (category is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Category"));

            category.Name = model.Name;

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyEdited, "category");
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            return await repository.AllReadonly<Category>()
                .Select(x => new CategoryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            var category = await repository.GetByIdAsync<Category>(id);

            if (category is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Category"));

            return new CategoryModel()
            {
                Id = id,
                Name = category.Name
            };
        }
    }
}
