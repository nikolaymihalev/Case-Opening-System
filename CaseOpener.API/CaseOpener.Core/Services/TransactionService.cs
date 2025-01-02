using CaseOpener.Core.Constants;
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
                return ReturnMessages.InvalidModel;

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

            return string.Format(ReturnMessages.SuccessfullyAdded, "transaction");
        }

        public async Task<string> DeleteTransactionAsync(int id)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null)
            {
                await repository.DeleteAsync<Transaction>(id);
                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyDeleted, "transaction");
            }

            return string.Format(ReturnMessages.DoesntExist, "Transaction");
        }

        public async Task<TransactionModel> GetTransactionByIdAsync(int id)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null) 
            {
                return new TransactionModel()
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    Type = transaction.Type,
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    Status = transaction.Status,
                };
            }

            throw new ArgumentException(string.Format(ReturnMessages.DoesntExist, "Transaction")); ;
        }

        public async Task<string> UpdateTransactionStatusAsync(int id, string newStatus)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null)
            {
                transaction.Status = newStatus;

                await repository.SaveChangesAsync();

                return string.Format(ReturnMessages.SuccessfullyUpdated, "transaction");
            }

            return string.Format(ReturnMessages.DoesntExist, "Transaction");
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
