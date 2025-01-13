using CaseOpener.Core.Constants;
using CaseOpener.Core.Models.User;
using CaseOpener.Core.Services;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Data;
using CaseOpener.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CaseOpener.UnitTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            userService = new UserService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task GetUserAsync_ExistingUser_ReturnsUserModel()
        {
            var user = new User
            {
                Id = "user123",
                Username = "testuser",
                Email = "test@example.com",
                DateJoined = DateTime.UtcNow,
                Balance = 1000m
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var result = await userService.GetUserAsync(user.Id);

            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Balance, result.Balance);
        }

        [Test]
        public async Task GetUserAsync_NonExistingUser_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await userService.GetUserAsync("nonexistentuser"));
        }

        [Test]
        public async Task GetUserByEmailAsync_ExistingUser_ReturnsUserModel()
        {
            var user = new User
            {
                Id = "user123",
                Username = "testuser",
                Email = "test@example.com",
                DateJoined = DateTime.UtcNow,
                Balance = 1000m
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var result = await userService.GetUserByEmailAsync(user.Email);

            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.Balance, result.Balance);
        }

        [Test]
        public async Task GetUserByEmailAsync_NonExistingUser_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await userService.GetUserByEmailAsync("nonexistentuser"));
        }

        [Test]
        public async Task GetUserRoleAsync_ExistingUser_ReturnsRole()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
            };

            var role = new Role()
            {
                Id = 1,
                Name = "User"
            };

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = 1,
            };

            await repository.AddAsync(role);
            await repository.AddAsync(user);
            await repository.AddAsync(userRole);
            await repository.SaveChangesAsync();

            var result = await userService.GetUserRoleAsync(user.Email);

            Assert.AreEqual("User", result);
        }

        [Test]
        public async Task GetUserRoleAsync_UserWithoutRole_ReturnsUserRole()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var result = await userService.GetUserRoleAsync(user.Email);

            Assert.AreEqual("User", result);
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessMessage()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "password123")
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var model = new LoginModel { Email = "test@example.com", Password = "password123" };

            var result = await userService.LoginAsync(model);

            Assert.AreEqual(ReturnMessages.SuccessfullyLoggedIn, result);
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "password123")
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var model = new LoginModel { Email = "test@example.com", Password = "wrongpassword" };

            Assert.ThrowsAsync<ArgumentException>(async () => await userService.LoginAsync(model));
        }

        [Test]
        public async Task RegisterAsync_ValidModel_ReturnsUserModel()
        {
            var model = new RegisterModel
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "password123"
            };

            var result = await userService.RegisterAsync(model);

            Assert.AreEqual(model.Username, result.Username);
            Assert.AreEqual(model.Email, result.Email);
        }

        [Test]
        public async Task RegisterAsync_ExistingEmail_ThrowsArgumentException()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                Username = "testuser",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "password123")
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var model = new RegisterModel
            {
                Username = "newuser",
                Email = "test@example.com", 
                Password = "password123"
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await userService.RegisterAsync(model));
        }

        [Test]
        public async Task UpdateUserBalanceAsync_ExistingUser_IncreasesBalance()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                Balance = 1000m
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var result = await userService.UpdateUserBalanceAsync(user.Id, "increase", 500m);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyModifiedBalance, "increased"), result);
            Assert.AreEqual(1500m, user.Balance);
        }

        [Test]
        public async Task UpdateUserBalanceAsync_ExistingUser_DecreasesBalance()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                Balance = 1000m
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var result = await userService.UpdateUserBalanceAsync(user.Id, "decrease", 500m);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyModifiedBalance, "decreased"), result);
            Assert.AreEqual(500m, user.Balance);
        }

        [Test]
        public async Task UpdateUserBalanceAsync_NonExistingUser_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await userService.UpdateUserBalanceAsync("nonexistentuser", "increase", 500m));
        }

        [Test]
        public async Task UpdateUserInformationAsync_ValidModel_ReturnsSuccessMessage()
        {
            var user = new User
            {
                Id = "user123",
                Email = "test@example.com",
                Username = "testuser"
            };

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            var model = new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = "updateduser"
            };

            var result = await userService.UpdateUserInformationAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyUpdated, "information"), result);
            Assert.AreEqual("updateduser", user.Username);
        }

        [Test]
        public async Task UpdateUserInformationAsync_NonExistingUser_ThrowsArgumentException()
        {
            var model = new UserModel
            {
                Id = "nonexistentuser",
                Email = "nonexistent@example.com",
                Username = "updateduser"
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await userService.UpdateUserInformationAsync(model));
        }
    }

}
