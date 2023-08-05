using Acerola.Domain.Accounts;

namespace Acerola.Application.Results
{
    public sealed class AccountResult
    {
        public Guid AccountId { get; }
        public double CurrentBalance { get; }
        public List<TransactionResult> Transactions { get; }

        public AccountResult(Guid accountId, double currentBalance, List<TransactionResult> transactions)
        {
            AccountId = accountId;
            CurrentBalance = currentBalance;
            Transactions = transactions;
        }

        public AccountResult(Account account)
        {
            AccountId = account.Id;
            CurrentBalance = account.GetCurrentBalance();

            List<TransactionResult> transactions = new List<TransactionResult>();
            foreach (ITransaction transaction in account.GetTransactions())
            {
                TransactionResult transactionResult = new TransactionResult(
                    transaction.Description, transaction.Amount, transaction.TransactionDate);
                transactions.Add(transactionResult);
            }

            Transactions = transactions;
        }
    }
}