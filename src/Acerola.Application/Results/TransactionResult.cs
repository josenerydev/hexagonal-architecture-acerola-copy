namespace Acerola.Application.Results
{
    public sealed class TransactionResult
    {
        public string Description { get; }
        public double Amount { get; }
        public DateTime TransactionDate { get; }

        public TransactionResult(string description, double amount, DateTime transactionDate)
        {
            Description = description;
            Amount = amount;
            TransactionDate = transactionDate;
        }
    }
}