using CaseOpener.Core.Constants;
using CaseOpener.Core.Models.User;
using CaseOpener.Core.Services;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Data;
using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace CaseOpener.UnitTests
{
    [TestFixture]
    public class AdminServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private AdminService adminService;

        [SetUp]
        public void Setup()
        {
            // Инициализация на in-memory контекст и репозиторий
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            adminService = new AdminService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            // Изчистване на контекста след всеки тест
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddRoleAsync_ShouldAddRole_WhenUserIsAdmin()
        {
            // Arrange
            var adminId = "admin123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            await context.SaveChangesAsync();

            var roleName = "NewRole";

            // Act
            var result = await adminService.AddRoleAsync(adminId, roleName);

            // Assert
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "role"), result);
            Assert.AreEqual(1, context.Roles.Count());
            Assert.AreEqual(roleName, context.Roles.First().Name);
        }

        [Test]
        public void AddRoleAsync_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var userId = "user123";
            var roleName = "NewRole";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.AddRoleAsync(userId, roleName));

            Assert.AreEqual(ReturnMessages.Unauthorized, ex.Message);
            Assert.AreEqual(0, context.Roles.Count());
        }

        [Test]
        public async Task AddUserToRoleAsync_ShouldAddUserRole_WhenRoleExists()
        {
            // Arrange
            var roleName = "ExistingRole";
            var userId = "user123";

            context.Roles.Add(new Role { Id = 3, Name = roleName });
            await context.SaveChangesAsync();

            // Act
            await adminService.AddUserToRoleAsync(userId, roleName);

            // Assert
            Assert.AreEqual(1, context.UserRoles.Count());
            Assert.AreEqual(userId, context.UserRoles.First().UserId);
        }

        [Test]
        public async Task AddUserToRoleAsync_ShouldDoNothing_WhenRoleDoesNotExist()
        {
            // Arrange
            var roleName = "NonExistingRole";
            var userId = "user123";

            // Act
            await adminService.AddUserToRoleAsync(userId, roleName);

            // Assert
            Assert.AreEqual(0, context.UserRoles.Count());
        }

        [Test]
        public async Task EditRoleAsync_ShouldEditRole_WhenUserIsAdminAndRoleExists()
        {
            // Arrange
            var adminId = "admin123";
            var roleId = 5;
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            context.Roles.Add(new Role { Id = roleId, Name = "OldRoleName" });
            await context.SaveChangesAsync();

            var updatedRole = new RoleModel { Id = roleId, Name = "NewRoleName" };

            // Act
            var result = await adminService.EditRoleAsync(adminId, updatedRole);

            // Assert
            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyEdited, "role"), result);
            Assert.AreEqual("NewRoleName", context.Roles.First().Name);
        }

        [Test]
        public void EditRoleAsync_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var userId = "user123";
            var roleId = 6;
            var updatedRole = new RoleModel { Id = roleId, Name = "NewRoleName" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.EditRoleAsync(userId, updatedRole));

            Assert.AreEqual(ReturnMessages.Unauthorized, ex.Message);
        }

        [Test]
        public async Task EditRoleAsync_ShouldThrowException_WhenRoleDoesNotExist()
        {
            // Arrange
            var adminId = "admin123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            await context.SaveChangesAsync();

            var updatedRole = new RoleModel { Id = 7, Name = "NewRoleName" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.EditRoleAsync(adminId, updatedRole));

            Assert.AreEqual(string.Format(ReturnMessages.DoesntExist, "Role"), ex.Message);
        }
        [Test]
        public async Task GetRolesAsync_ShouldReturnRoles_WhenUserIsAdmin()
        {
            // Arrange
            var adminId = "admin123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            context.Roles.AddRange(new Role { Id = 1, Name = "Role1" }, new Role { Id = 2, Name = "Role2" });
            await context.SaveChangesAsync();

            // Act
            var roles = await adminService.GetRolesAsync(adminId);

            // Assert
            Assert.AreEqual(2, roles.Count());
            Assert.AreEqual("Role1", roles.First().Name);
        }

        [Test]
        public void GetRolesAsync_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var userId = "user123";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.GetRolesAsync(userId));

            Assert.AreEqual(ReturnMessages.Unauthorized, ex.Message);
        }

        [Test]
        public async Task GetUsersAsync_ShouldReturnUsers_WhenUserIsAdmin()
        {
            // Arrange
            var adminId = "admin123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            context.Users.AddRange(
                new User { Id = "user1", Username = "User1", Email = "user1@example.com", Balance = 100, DateJoined = DateTime.Now },
                new User { Id = "user2", Username = "User2", Email = "user2@example.com", Balance = 200, DateJoined = DateTime.Now }
            );
            await context.SaveChangesAsync();

            // Act
            var users = await adminService.GetUsersAsync(adminId);

            // Assert
            Assert.AreEqual(2, users.Count());
            Assert.AreEqual("User1", users.First().Username);
        }

        [Test]
        public void GetUsersAsync_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var userId = "user123";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.GetUsersAsync(userId));

            Assert.AreEqual(ReturnMessages.Unauthorized, ex.Message);
        }

        [Test]
        public async Task GetUserTransactionsAsync_ShouldReturnTransactions_WhenUserIsAdmin()
        {
            // Arrange
            var adminId = "admin123";
            var userId = "user123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            context.Transactions.AddRange(
                new Transaction { Id = 1, UserId = userId, Amount = 100, Type = "Deposit", Date = DateTime.Now, Status = "Success" },
                new Transaction { Id = 2, UserId = userId, Amount = 50, Type = "Withdraw", Date = DateTime.Now, Status = "Pending" }
            );
            await context.SaveChangesAsync();

            // Act
            var transactions = await adminService.GetUserTransactionsAsync(adminId, userId);

            // Assert
            Assert.AreEqual(2, transactions.Count());
            Assert.AreEqual("Deposit", transactions.First().Type);
        }

        [Test]
        public void GetUserTransactionsAsync_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var adminId = "user123";
            var userId = "user456";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await adminService.GetUserTransactionsAsync(adminId, userId));

            Assert.AreEqual(ReturnMessages.Unauthorized, ex.Message);
        }

        [Test]
        public async Task CheckUserIsAdmin_ShouldReturnTrue_WhenUserHasAdminRole()
        {
            // Arrange
            var adminId = "admin123";
            context.UserRoles.Add(new UserRole { UserId = adminId, RoleId = 2 });
            await context.SaveChangesAsync();

            // Act
            var isAdmin = await adminService.CheckUserIsAdmin(adminId);

            // Assert
            Assert.IsTrue(isAdmin);
        }

        [Test]
        public async Task CheckUserIsAdmin_ShouldReturnFalse_WhenUserDoesNotHaveAdminRole()
        {
            // Arrange
            var userId = "user123";
            context.UserRoles.Add(new UserRole { UserId = userId, RoleId = 1 });
            await context.SaveChangesAsync();

            // Act
            var isAdmin = await adminService.CheckUserIsAdmin(userId);

            // Assert
            Assert.IsFalse(isAdmin);
        }

    }
}
