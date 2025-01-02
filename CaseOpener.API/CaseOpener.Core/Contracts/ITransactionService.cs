using CaseOpener.Core.Models.Transaction;

namespace CaseOpener.Core.Contracts
{
    public interface ITransactionService
    {
        Task<string> AddTransactionAsync(TransactionModel model);
        Task<string> DeleteTransactionAsync(string adminId, int id);
        Task<string> UpdateTransactionStatusAsync(string adminId, int id, string newStatus);
        Task<TransactionModel> GetTransactionByIdAsync(string userId, int id);
    }
}
