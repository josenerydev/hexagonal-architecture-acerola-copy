using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Accounts;

using Microsoft.EntityFrameworkCore;

namespace Acerola.Infrastructure.EntityFrameworkDataAccess.Queries
{
    public class AccountsQueries : IAccountsQueries
    {
        private readonly Context _context;

        public AccountsQueries(Context context)
        {
            _context = context;
        }

        public async Task<AccountResult> GetAccount(Guid accountId)
        {
            Entities.Account account = await _context
                .Accounts
                .FindAsync(accountId);

            if (account == null)
                throw new AccountNotFoundException($"The account {accountId} does not exists or is not processed yet.");

            List<Entities.Credit> credits = await _context
                .Credits
                .Where(e => e.AccountId == accountId)
                .ToListAsync();

            List<Entities.Debit> debits = await _context
                .Debits
                .Where(e => e.AccountId == accountId)
                .ToListAsync();

            List<ITransaction> transactions = new List<ITransaction>();

            foreach (Entities.Credit transactionData in credits)
            {
                Credit transaction = Credit.Load(
                    transactionData.Id,
                    transactionData.AccountId,
                    transactionData.Amount,
                    transactionData.TransactionDate);

                transactions.Add(transaction);
            }

            foreach (Entities.Debit transactionData in debits)
            {
                Debit transaction = Debit.Load(
                    transactionData.Id,
                    transactionData.AccountId,
                    transactionData.Amount,
                    transactionData.TransactionDate);

                transactions.Add(transaction);
            }

            var orderedTransactions = transactions.OrderBy(o => o.TransactionDate).ToList();

            TransactionCollection transactionCollection = new TransactionCollection();
            transactionCollection.Add(orderedTransactions);

            Account result = Account.Load(
                account.Id,
                account.CustomerId,
                transactionCollection);

            AccountResult re = new AccountResult(result);
            return re;
        }
    }
}