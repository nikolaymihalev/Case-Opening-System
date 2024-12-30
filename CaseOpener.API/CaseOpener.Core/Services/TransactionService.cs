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
        private readonly IAdminService adminService;

        public TransactionService(
            IRepository _repository,
            IAdminService _adminService)
        {
            repository = _repository;
            adminService = _adminService;
        }

        public async Task<string> AddTransactionAsync(TransactionModel model)
        {
            if(IsValidEnumValue<TransactionType>(model.Type) == false)
            {
                return ReturnMessages.INVALID_MODEL;
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

            return string.Format(ReturnMessages.SUCCESSFYLLY_ADDED, "transaction");
        }

        public async Task<string> DeleteTransactionAsync(string adminId, int id)
        {
            if(await adminService.CheckUserIsAdmin(adminId))
            {
                var transaction = await repository.GetByIdAsync<Transaction>(id);

                if (transaction != null)
                {
                    await repository.DeleteAsync<Transaction>(id);
                    await repository.SaveChangesAsync();

                    return string.Format(ReturnMessages.SUCCESSFULLY_DELETED, "transaction");
                }

                return string.Format(ReturnMessages.DOESNT_EXIST, "Transaction");
            }

            return ReturnMessages.UNAUTHORIZED;
        }

        public async Task<TransactionModel?> GetTransactionByIdAsync(string userId, int id)
        {
            var transaction = await repository.GetByIdAsync<Transaction>(id);

            if (transaction != null) 
            {
                if (transaction.UserId == userId || await adminService.CheckUserIsAdmin(userId))
                    return new TransactionModel()
                    {
                        Id = transaction.Id,
                        UserId = userId,
                        Type = transaction.Type,
                        Amount = transaction.Amount,
                        Date = transaction.Date,
                        Status = transaction.Status,
                    };
                else
                    throw new ArgumentException(ReturnMessages.UNAUTHORIZED);
            }              

            return null;
        }

        public async Task<string> UpdateTransactionStatusAsync(string adminId, int id, string newStatus)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            { 
                var transaction = await repository.GetByIdAsync<Transaction>(id);

                if (transaction != null)
                {
                    transaction.Status = newStatus;

                    await repository.SaveChangesAsync();

                    return string.Format(ReturnMessages.SUCCESSFULLY_UPDATED, "transaction");
                }

                return string.Format(ReturnMessages.DOESNT_EXIST, "Transaction");
            }

            return ReturnMessages.UNAUTHORIZED;
        }

        private bool IsValidEnumValue<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value, true, out _);
        }
    }
}
