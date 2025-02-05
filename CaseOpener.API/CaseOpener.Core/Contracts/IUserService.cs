using CaseOpener.Core.Models.User;

namespace CaseOpener.Core.Contracts
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginModel model);
        Task<UserModel> RegisterAsync(RegisterModel model);
        Task<string> UpdateUserInformationAsync(string userId, string username);
        Task<string> UpdateUserBalanceAsync(string userId, string operation, decimal amount);
        Task<UserModel> GetUserByEmailAsync(string email);
        Task<UserModel> GetUserAsync(string userId);
        Task<string> GetUserRoleAsync(string email);
    }
}
