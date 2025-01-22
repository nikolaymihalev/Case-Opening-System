using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Models.Item;

namespace CaseOpener.Core.Contracts
{
    public interface ICaseService
    {
        Task<IEnumerable<CasePageModel>> GetAllCasesAsync(string? name = null);
        Task<IEnumerable<CaseOpeningModel>> GetUserOpenedCasesAsync(string userId);
        Task<IEnumerable<CaseItemModel>> GetCaseItemsProbabilities(int caseId);
        Task<CaseModel> GetCaseByIdAsync(int id);
        Task<string> AddCaseAsync(CaseFormModel model);
        Task<string> EditCaseAsync(CaseFormModel model);
        Task<string> DeleteCaseAsync(int id);
        Task<string> AddItemToCaseAsync(int caseId, int itemId, double probability);
        Task<ItemModel> OpenCaseAsync(int caseId, string userId);
        Task<int> DoesUserHaveCaseAsync(string userId, int caseId);
        Task<string> BuyCaseAsync(int caseId, string userId, int quantity);
        Task<IEnumerable<CaseUserModel>> GetUsersCasesAsync(string userId);
    }
}
