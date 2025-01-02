using CaseOpener.Core.Models.Case;

namespace CaseOpener.Core.Contracts
{
    public interface ICaseService
    {
        Task<IEnumerable<CaseModel>> GetAllCasesAsync();
        Task<CaseModel> GetCaseByIdAsync(int id);
        Task SubscribeUserToDailyRewardAsync(string userId);
        Task<string> AddCaseAsync(CaseFormModel model, string adminId);
        Task<string> EditCaseAsync(CaseFormModel model, string adminId);
        Task<string> DeleteCaseAsync(int id, string adminId);
        Task<string> OpenCaseAsync(int caseId, string userId);
        Task<string> OpenDailyRewardAsync(string userId);
    }
}
