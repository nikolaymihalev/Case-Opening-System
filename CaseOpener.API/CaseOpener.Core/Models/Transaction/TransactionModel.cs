namespace CaseOpener.Core.Models.Transaction
{
    /// <summary>
    /// Model for Transaction
    /// </summary>
    public class TransactionModel
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Type of the transaction.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Amount of the transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date of the transaction.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Status of the transaction.
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
