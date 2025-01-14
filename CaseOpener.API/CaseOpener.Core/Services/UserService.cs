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

        public UserService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<UserModel> GetUserAsync(string userId)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if(user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User")); ;

            return new UserModel()
            {
                Id = userId,
                Username = user.Username,
                Balance = user.Balance,
                DateJoined = user.DateJoined,
                Email = user.Email,
            };
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var user = await repository.AllReadonly<User>().FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User")); ;

            return new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                Balance = user.Balance,
                DateJoined = user.DateJoined,
                Email = user.Email,
            };
        }

        public async Task<string> GetUserRoleAsync(string email)
        {
            var roles = await repository.AllReadonly<UserRole>().Include(x => x.User).Where(x => x.User.Email == email).ToListAsync();

            if (roles.Count > 1)
                return "Admin";

            return "User";
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await repository.AllReadonly<User>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Success)
                return ReturnMessages.SuccessfullyLoggedIn;

            throw new ArgumentException(ReturnMessages.InvalidPassword);
        }

        public async Task<UserModel> RegisterAsync(RegisterModel model)
        {
            var user = await repository.AllReadonly<User>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user != null)
                throw new ArgumentException(string.Format(ReturnMessages.AlreadyExist, "User"));

            user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.Username,
                Email = model.Email,
                DateJoined = DateTime.UtcNow,
                Balance = 1000m
            };

            var passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            return new UserModel()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DateJoined = user.DateJoined,
                Balance = user.Balance
            };
        }

        public async Task<string> UpdateUserBalanceAsync(string userId, string operation, decimal amount)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user is null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            if(operation == "increase")
            {
                user.Balance += amount;

                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyModifiedBalance, "increased");
            }
            else
            {
                user.Balance -= amount;

                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyModifiedBalance, "decreased");
            }
        }

        public async Task<string> UpdateUserInformationAsync(string userId, string username)
        {
            var user = await repository.GetByIdAsync<User>(userId);

            if (user == null)
                throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "User"));

            user.Username = username;

            await repository.SaveChangesAsync();

            return string.Format(ReturnMessages.SuccessfullyUpdated, "information");
        }
    }
}
