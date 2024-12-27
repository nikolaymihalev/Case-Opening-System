using CaseOpener.Core.Contracts;
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

        public async Task AddUserToRoleAsync(string userId, int roleId)
        {
            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = roleId
            };

            await repository.AddAsync(userRole);
            await repository.SaveChangesAsync();
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

            return new List<RoleModel>();
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
