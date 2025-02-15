﻿using CaseOpener.Core.Models.Transaction;
using CaseOpener.Core.Models.User;

namespace CaseOpener.Core.Contracts
{
    public interface IAdminService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync(string adminId);
        Task<IEnumerable<TransactionModel>> GetUserTransactionsAsync(string adminId, string userId);
        Task<IEnumerable<RoleModel>> GetRolesAsync(string adminId);
        Task AddUserToRoleAsync(string userId, string roleName);
        Task<string> AddRoleAsync(string adminId, string roleName);
        Task<string> EditRoleAsync(string adminId, RoleModel model);
        Task<bool> CheckUserIsAdmin(string userId);
    }
}
