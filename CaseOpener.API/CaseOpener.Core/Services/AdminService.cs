using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Transaction;
using CaseOpener.Core.Models.User;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository repository;

        public AdminService(IRepository _repository)
        {
            repository = _repository;    
        }

        public async Task<string> AddRoleAsync(string adminId, RoleModel model)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized") 
            {
                var role = new Role()
                {
                    Name = model.Name
                };

                await repository.AddAsync(role);
                await repository.SaveChangesAsync();

                return "Successfully added new role!";
            }

            return result;           
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            var role = await repository.All<Role>()
                .FirstOrDefaultAsync(x => x.Name == roleName);

            if(role != null)
            {
                var userRole = new UserRole()
                {
                    UserId = userId,
                    RoleId = role.Id
                };

                await repository.AddAsync(userRole);
                await repository.SaveChangesAsync();
            }            
        }

        public async Task<string> EditRoleAsync(string adminId, RoleModel model)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized")
            {
                var role = await repository.GetByIdAsync<Role>(model.Id);

                if(role != null)
                {
                    role.Name = model.Name;

                    await repository.SaveChangesAsync();

                    return "Successfully edited role!";
                }   
                else
                {
                    return "Role doesn't exist!";
                }                
            }

            return result;
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync(string adminId)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized")
            {
                return await repository.AllReadonly<Role>()
                    .Select(x => new RoleModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToListAsync();
            }

            throw new ArgumentException("Unauthorized!");
        }

        public async Task<UserModel?> GetUserInformationAsync(string adminId, string userId)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized")
            {
                var user = await repository.GetByIdAsync<User>(userId);

                if (user is null)
                    return null;

                return new UserModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Balance = user.Balance,
                    DateJoined = user.DateJoined
                };
            }

            throw new ArgumentException("Unauthorized!");
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string adminId)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized")
            {
                return await repository.AllReadonly<User>()
                    .Where(x => x.Id != adminId)
                    .Select(x => new UserModel()
                    {
                        Id = x.Id,
                        Username = x.Username,
                        Email = x.Email,
                        Balance = x.Balance,
                        DateJoined = x.DateJoined
                    })
                    .ToListAsync();
            }

            throw new ArgumentException("Unauthorized!");
        }

        public async Task<IEnumerable<TransactionModel>> GetUserTransactionsAsync(string adminId, string userId)
        {
            string result = CheckUserId(adminId).Result;

            if (result == "Authorized")
            {
                return await repository.AllReadonly<Transaction>()
                    .Where(x => x.UserId == userId)
                    .Select(x => new TransactionModel()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        Type = x.Type,
                        Amount = x.Amount,
                        Date = x.Date,
                        Status = x.Status
                    })
                    .ToListAsync();
            }

            throw new ArgumentException("Unauthorized!");
        }

        private async Task<string> CheckUserId(string userId)
        {
            var userRoles = await repository.AllReadonly<UserRole>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (userRoles.Any(x => x.RoleId == 2))
                return "Authorized!";
            else
                return "Unauthorized";
        }
    }
}
