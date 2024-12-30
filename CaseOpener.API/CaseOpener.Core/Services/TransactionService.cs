using CaseOpener.Core.Contracts;
using CaseOpener.Core.Enums;
using CaseOpener.Core.Models.Transaction;
using CaseOpener.Infrastructure.Common;
using CaseOpener.Infrastructure.Models;

namespace CaseOpener.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository repository;

        public TransactionService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<string> AddTransactionAsync(TransactionModel model)
        {
            if(IsValidEnumValue<TransactionType>(model.Type) == false)
            {
                return "Invalid model!";
            }

            var transaction = new Transaction()
            {
                UserId = model.UserId,
                Type = model.Type,
                Amount = model.Amount,
                Date = DateTime.UtcNow,
                Status = model.Status,
            };

            await repository.AddAsync(transaction);
            await repository.SaveChangesAsync();

            return "Successfully added new transaction!";
        }

        public async Task<string> DeleteTransactionAsync(string adminId, int id)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null)
            {
                await repository.DeleteAsync<Transaction>(id);
                await repository.SaveChangesAsync();

                return "Successfully deleted transaction";
            }

            return "Transaction doesn't exist!";
        }

        public async Task<TransactionModel?> GetTransactionByIdAsync(string userId, int id)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null && transaction.UserId == userId)
                return new TransactionModel()
                {
                    Id = transaction.Id,
                    UserId = userId,
                    Type = transaction.Type,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Status = transaction.Status,
                };

            return null;
        }

        public async Task<string> UpdateTransactionStatusAsync(string adminId, int id, string newStatus)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null)
            {
                transaction.Status = newStatus;

                await repository.SaveChangesAsync();

                return "Successfully updated transaction";
            }

            return "Transaction doesn't exist!";
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
