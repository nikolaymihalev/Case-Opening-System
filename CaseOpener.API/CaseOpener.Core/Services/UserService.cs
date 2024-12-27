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

        public UserService(IRepository _repository)
        {
            repository = _repository;
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
                return "User already exists!";

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return "User successfully logged in!";
            }

            return "Invalid password";
        }

        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var user = await repository.AllReadonly<User>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user != null)
                return "User already exists!";

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

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            return "You was successfully registered!";
        }

        public async Task<string> UpdateUserBalanceAsync(string userId, string operation, decimal amount)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                return "User doesn't exist!";

            if(operation == "increase")
            {
                user.Balance += amount;

                await repository.SaveChangesAsync();

                return $"Successfully increased balance!";
            }
            else
            {
                user.Balance -= amount;

                await repository.SaveChangesAsync();

                return $"Successfully decreased balance!";
            }
        }

        public async Task<string> UpdateUserInformationAsync(UserModel model)
        {
            var user = await repository.All<User>()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || user.Email != model.Email)
                return "User doesn't exist!";

            user.Username = model.Username;

            await repository.SaveChangesAsync();

            return "Successfully updated information!";
        }
    }
}
