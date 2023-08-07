using Acerola.Application.Queries;
using Acerola.Application.Results;

using Raven.Client.Documents;

namespace Acerola.Infrastructure.Queries
{
    public class CustomersQueries : ICustomersQueries
    {
        private readonly IDocumentStore _documentStore;
        private readonly IAccountsQueries _accountsQueries;

        public CustomersQueries(IDocumentStore documentStore, IAccountsQueries accountsQueries)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
            _accountsQueries = accountsQueries;
        }

        public async Task<CustomerResult> GetCustomer(Guid customerId)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Customer customer = await session.LoadAsync<Entities.Customer>(customerId.ToString());

                if (customer == null)
                    throw new CustomerNotFoundException($"The customer {customerId} does not exist or is not processed yet.");

                List<Entities.Account> accounts = await session
                    .Query<Entities.Account>()
                    .Where(e => e.CustomerId == customerId)
                    .ToListAsync();

                List<AccountResult> accountsResult = new List<AccountResult>();

                foreach (Entities.Account account in accounts)
                {
                    AccountResult accountResult = await _accountsQueries.GetAccount(account.Id);
                    accountsResult.Add(accountResult);
                }

                CustomerResult customerResult = new CustomerResult(
                    customer.Id, customer.Name, customer.SSN,
                    accountsResult);

                return customerResult;
            }
        }
    }
}