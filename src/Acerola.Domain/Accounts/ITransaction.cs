using Acerola.Domain.ValueObjects;

namespace Acerola.Domain.Accounts
{
    public interface ITransaction
    {
        Amount Amount { get; }
        string Description { get; }
        DateTime TransactionDate { get; }
    }
}