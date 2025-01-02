using CaseOpener.Core.Models.User;

namespace CaseOpener.Core.Contracts
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginModel model);
        Task<string> RegisterAsync(RegisterModel model);
        Task<string> UpdateUserInformationAsync(UserModel model);
        Task<string> UpdateUserBalanceAsync(string userId, string operation, decimal amount);
        Task<UserModel> GetUserAsync(string userId);
    }
}
