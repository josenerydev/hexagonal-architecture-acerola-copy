using Acerola.Application.Repositories;
using Acerola.Domain.Accounts;

using Raven.Client.Documents;

namespace Acerola.Infrastructure.Repositories
{
    public class AccountRepository : IAccountReadOnlyRepository, IAccountWriteOnlyRepository
    {
        private readonly IDocumentStore _documentStore;

        public AccountRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task Add(Account account, Credit credit)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Account accountEntity = new Entities.Account
                {
                    Id = account.Id,
                    CustomerId = account.CustomerId
                };

                Entities.Credit creditEntity = new Entities.Credit
                {
                    Id = credit.Id,
                    AccountId = credit.AccountId,
                    Amount = credit.Amount,
                    TransactionDate = credit.TransactionDate
                };

                await session.StoreAsync(accountEntity);
                await session.StoreAsync(creditEntity);

                await session.SaveChangesAsync();
            }
        }

        public async Task Delete(Account account)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var creditToDelete = await session.Query<Entities.Credit>()
                                                  .Where(c => c.AccountId == account.Id)
                                                  .ToListAsync();

                foreach (var credit in creditToDelete)
                {
                    session.Delete(credit);
                }

                var debitToDelete = await session.Query<Entities.Debit>()
                                                  .Where(d => d.AccountId == account.Id)
                                                  .ToListAsync();

                foreach (var debit in debitToDelete)
                {
                    session.Delete(debit);
                }

                session.Delete(account.Id);

                await session.SaveChangesAsync();
            }
        }

        public async Task<Account> Get(Guid id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Account account = await session.LoadAsync<Entities.Account>(id.ToString());
                if (account == null)
                    throw new AccountNotFoundException($"The account {id} does not exist or is not processed yet.");

                List<Entities.Credit> credits = await session
                    .Query<Entities.Credit>()
                    .Where(c => c.AccountId == id)
                    .ToListAsync();

                List<Entities.Debit> debits = await session
                    .Query<Entities.Debit>()
                    .Where(d => d.AccountId == id)
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

                return result;
            }
        }

        public async Task Update(Account account, Credit credit)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Credit creditEntity = new Entities.Credit
                {
                    Id = credit.Id,
                    AccountId = credit.AccountId,
                    Amount = credit.Amount,
                    TransactionDate = credit.TransactionDate
                };

                await session.StoreAsync(creditEntity);

                await session.SaveChangesAsync();
            }
        }

        public async Task Update(Account account, Debit debit)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Debit debitEntity = new Entities.Debit
                {
                    Id = debit.Id,
                    AccountId = debit.AccountId,
                    Amount = debit.Amount,
                    TransactionDate = debit.TransactionDate
                };

                await session.StoreAsync(debitEntity);

                await session.SaveChangesAsync();
            }
        }
    }
}