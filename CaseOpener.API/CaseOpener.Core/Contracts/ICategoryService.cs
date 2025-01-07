using CaseOpener.Core.Models.Case;

namespace CaseOpener.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<string> AddAsync(CategoryFormModel model);
        Task<string> EditAsync(CategoryFormModel model);
        Task<CategoryModel> GetByIdAsync(int id);
    }
}
