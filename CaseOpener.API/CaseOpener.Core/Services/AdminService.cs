using CaseOpener.Core.Constants;
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
            if (await CheckUserIsAdmin(adminId)) 
            {
                var role = new Role()
                {
                    Name = model.Name
                };

                await repository.AddAsync(role);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SUCCESSFYLLY_ADDED, "role");
            }

            return ReturnMessages.UNAUTHORIZED;           
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
            if (await CheckUserIsAdmin(adminId))
            {
                var role = await repository.GetByIdAsync<Role>(model.Id);

                if(role != null)
                {
                    role.Name = model.Name;

                    await repository.SaveChangesAsync();

                    return string.Format(ReturnMessages.SUCCESSFULLY_EDITED, "role");
                }   
                else
                {
                    return string.Format(ReturnMessages.DOESNT_EXIST, "Role");
                }                
            }

            return ReturnMessages.UNAUTHORIZED;
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync(string adminId)
        {
            if (await CheckUserIsAdmin(adminId))
            {
                return await repository.AllReadonly<Role>()
                    .Select(x => new RoleModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToListAsync();
            }

            throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
        }

        public async Task<UserModel?> GetUserInformationAsync(string adminId, string userId)
        {
            if (await CheckUserIsAdmin(adminId))
            {
                var user = await repository.GetByIdAsync<User>(userId);

                if (user is null)
                    throw new ArgumentException(string.Format(ReturnMessages.DOESNT_EXIST, "User"));

                return new UserModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Balance = user.Balance,
                    DateJoined = user.DateJoined
                };
            }

            throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync(string adminId)
        {
            if (await CheckUserIsAdmin(adminId))
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

            throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
        }

        public async Task<IEnumerable<TransactionModel>> GetUserTransactionsAsync(string adminId, string userId)
        {
            if (await CheckUserIsAdmin(adminId))
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

            throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
        }

        public async Task<bool> CheckUserIsAdmin(string userId)
        {
            var userRoles = await repository.AllReadonly<UserRole>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return userRoles.Any(x => x.RoleId == 2);
        }
    }
}
