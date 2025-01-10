using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Models.Item;

namespace CaseOpener.Core.Contracts
{
    public interface ICaseService
    {
        Task<IEnumerable<CasePageModel>> GetAllCasesAsync(string? name = null);
        Task<CaseModel> GetCaseByIdAsync(int id);
        Task SubscribeUserToDailyRewardAsync(string userId);
        Task<string> AddCaseAsync(CaseFormModel model);
        Task<string> EditCaseAsync(CaseFormModel model);
        Task<string> DeleteCaseAsync(int id);
        Task<string> AddItemToCaseAsync(int caseId, int itemId, double probability);
        Task<ItemModel> OpenCaseAsync(int caseId, string userId);
        Task<ItemModel> OpenDailyRewardAsync(string userId);
        Task<bool> DoesUserHaveCase(string userId, int caseId);
    }
}
