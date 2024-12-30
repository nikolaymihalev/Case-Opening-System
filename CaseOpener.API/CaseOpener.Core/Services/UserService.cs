using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.User;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IAdminService adminService;

        public UserService(
            IRepository _repository,
            IAdminService _adminService)
        {
            repository = _repository;
            adminService = _adminService;
        }

        public async Task<UserModel?> GetUserAsync(string userId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if(user is null)
                return null;

            return new UserModel()
            {
                Id = userId,
                Username = user.Username,
                Balance = user.Balance,
                DateJoined = user.DateJoined,
                Email = user.Email,
            };
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await repository.AllReadonly<User>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user != null)
                return string.Format(ReturnMessages.ALREADY_EXIST, "User");

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return ReturnMessages.SUCCESSFULLY_LOGGED_IN;
            }

            return ReturnMessages.INVALID_PASSWORD;
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var user = await repository.AllReadonly<User>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user != null)
                return string.Format(ReturnMessages.ALREADY_EXIST, "User");

            user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.Username,
                Email = model.Username,
                DateJoined = DateTime.UtcNow,
                Balance = 1000m
            };

            var passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            await adminService.AddUserToRoleAsync(user.Id, "User");

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            return ReturnMessages.SUCCESSFULLY_REGISTERED;
        }

        public async Task<string> UpdateUserBalanceAsync(string userId, string operation, decimal amount)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                return string.Format(ReturnMessages.DOESNT_EXIST, "User");

            if(operation == "increase")
            {
                user.Balance += amount;

                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SUCCESSFULLY_MODIFIED_BALANCE, "increased");
            }
            else
            {
                user.Balance -= amount;

                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SUCCESSFULLY_MODIFIED_BALANCE, "decreased");
            }
        }

        public async Task<string> UpdateUserInformationAsync(UserModel model)
        {
            var user = await repository.All<User>()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || user.Email != model.Email)
                return string.Format(ReturnMessages.DOESNT_EXIST, "User");

            user.Username = model.Username;

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SUCCESSFULLY_UPDATED, "information");
        }
    }
}
