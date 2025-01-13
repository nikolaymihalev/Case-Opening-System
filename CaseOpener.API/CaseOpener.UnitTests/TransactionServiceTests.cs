using CaseOpener.Core.Constants;
using CaseOpener.Core.Models.Transaction;
using CaseOpener.Core.Services;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Data;
using CaseOpener.Infrastructure.Models;

namespace CaseOpener.UnitTests
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private ApplicationDbContext context;
        private Repository repository;
        private TransactionService transactionService;

        [SetUp]
        public void Setup()
        {
            context = InMemoryDbContextFactory.Create();
            repository = new Repository(context);
            transactionService = new TransactionService(repository);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddTransactionAsync_ValidTransaction_ReturnsSuccessMessage()
        {
            var model = new TransactionModel
            {
                UserId = "user123",
                Type = "Deposit",
                Amount = 100.50m,
                Status = "Completed"
            };

            var result = await transactionService.AddTransactionAsync(model);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyAdded, "transaction"), result);
        }

        [Test]
        public async Task AddTransactionAsync_InvalidTransactionType_ReturnsInvalidModelMessage()
        {
            var model = new TransactionModel
            {
                UserId = "user123",
                Type = "InvalidType", 
                Amount = 100.50m,
                Status = "Completed"
            };

            var result = await transactionService.AddTransactionAsync(model);

            Assert.AreEqual(ReturnMessages.InvalidModel, result);
        }

        [Test]
        public async Task DeleteTransactionAsync_ExistingTransaction_ReturnsSuccessMessage()
        {
            var transaction = new Transaction
            {
                UserId = "user123",
                Type = "Deposit",
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Status = "Completed"
            };

            await repository.AddAsync(transaction);
            await repository.SaveChangesAsync();

            var result = await transactionService.DeleteTransactionAsync(transaction.Id);

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyDeleted, "transaction"), result);
        }

        [Test]
        public async Task DeleteTransactionAsync_NonExistingTransaction_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await transactionService.DeleteTransactionAsync(999)); 
        }

        [Test]
        public async Task GetTransactionByIdAsync_ExistingTransaction_ReturnsTransactionModel()
        {
            var transaction = new Transaction
            {
                UserId = "user123",
                Type = "Deposit",
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Status = "Completed"
            };

            await repository.AddAsync(transaction);
            await repository.SaveChangesAsync();

            var result = await transactionService.GetTransactionByIdAsync(transaction.Id);

            Assert.AreEqual(transaction.Id, result.Id);
            Assert.AreEqual(transaction.UserId, result.UserId);
            Assert.AreEqual(transaction.Amount, result.Amount);
            Assert.AreEqual(transaction.Status, result.Status);
        }

        [Test]
        public async Task GetTransactionByIdAsync_NonExistingTransaction_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await transactionService.GetTransactionByIdAsync(999)); 
        }

        [Test]
        public async Task UpdateTransactionStatusAsync_ValidStatus_ReturnsSuccessMessage()
        {
            var transaction = new Transaction
            {
                UserId = "user123",
                Type = "Deposit",
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Status = "Pending"
            };

            await repository.AddAsync(transaction);
            await repository.SaveChangesAsync();

            var result = await transactionService.UpdateTransactionStatusAsync(transaction.Id, "Completed");

            Assert.AreEqual(string.Format(ReturnMessages.SuccessfullyUpdated, "transaction"), result);
        }

        [Test]
        public async Task UpdateTransactionStatusAsync_InvalidStatus_ThrowsArgumentException()
        {
            var transaction = new Transaction
            {
                UserId = "user123",
                Type = "Deposit",
                Amount = 100.50m,
                Date = DateTime.UtcNow,
                Status = "Pending"
            };

            await repository.AddAsync(transaction);
            await repository.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async () => await transactionService.UpdateTransactionStatusAsync(transaction.Id, "InvalidStatus"));
        }

        [Test]
        public async Task UpdateTransactionStatusAsync_NonExistingTransaction_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await transactionService.UpdateTransactionStatusAsync(999, "Completed"));
        }
    }

}
