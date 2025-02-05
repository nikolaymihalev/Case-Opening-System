using CaseOpener.Core.Models.Transaction;

namespace CaseOpener.Core.Contracts
{
    public interface ITransactionService
    {
        Task<string> AddTransactionAsync(TransactionModel model);
        Task<string> DeleteTransactionAsync(int id);
        Task<string> UpdateTransactionStatusAsync(int id, string newStatus);
        Task<TransactionModel> GetTransactionByIdAsync(int id);
    }
}
