using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.Domain.Accounts;

using Raven.Client.Documents;

using Account = Acerola.Domain.Accounts.Account;
using Credit = Acerola.Domain.Accounts.Credit;
using Debit = Acerola.Domain.Accounts.Debit;

namespace Acerola.Infrastructure.Queries
{
    public class AccountsQueries : IAccountsQueries
    {
        private readonly IDocumentStore _documentStore;

        public AccountsQueries(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task<AccountResult> GetAccount(Guid accountId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Account account = await session.LoadAsync<Entities.Account>(accountId.ToString());
                if (account == null)
                    throw new AccountNotFoundException($"The account {accountId} does not exist or is not processed yet.");

                List<Entities.Credit> credits = await session
                    .Query<Entities.Credit>()
                    .Where(e => e.AccountId == accountId)
                    .ToListAsync();

                List<Entities.Debit> debits = await session
                    .Query<Entities.Debit>()
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
}