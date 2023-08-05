using Acerola.Application.Results;
using Acerola.Domain.Accounts;
using Acerola.Domain.ValueObjects;

namespace Acerola.Application.Commands.Withdraw
{
    public class WithdrawResult
    {
        public TransactionResult Transaction { get; }
        public double UpdatedBalance { get; }

        public WithdrawResult(Debit transaction, Amount updatedBalance)
        {
            Transaction = new TransactionResult(
                transaction.Description,
                transaction.Amount,
                transaction.TransactionDate);

            UpdatedBalance = updatedBalance;
        }
    }
}