using Acerola.Application.Repositories;
using Acerola.Domain.Customers;

using Raven.Client.Documents;

namespace Acerola.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerReadOnlyRepository, ICustomerWriteOnlyRepository
    {
        private readonly IDocumentStore _documentStore;

        public CustomerRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task Add(Customer customer)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Customer customerEntity = new Entities.Customer()
                {
                    Id = customer.Id.ToString(),
                    Name = customer.Name,
                    SSN = customer.SSN
                };

                await session.StoreAsync(customerEntity);

                await session.SaveChangesAsync();
            }
        }

        public async Task<Customer> Get(Guid id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Customer customer = await session.LoadAsync<Entities.Customer>(id.ToString());
                if (customer == null)
                    throw new CustomerNotFoundException($"The customer {id} does not exist or is not processed yet.");

                List<string> accounts = await session
                    .Query<Entities.Account>()
                    .Where(a => a.CustomerId == id.ToString())
                    .Select(a => a.Id)
                    .ToListAsync();

                AccountCollection accountCollection = new AccountCollection();
                foreach (var accountId in accounts)
                    accountCollection.Add(Guid.Parse(accountId));

                return Customer.Load(Guid.Parse(customer.Id), customer.Name, customer.SSN, accountCollection);
            }
        }

        public async Task Update(Customer customer)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                Entities.Customer customerEntity = new Entities.Customer()
                {
                    Id = customer.Id.ToString(),
                    Name = customer.Name,
                    SSN = customer.SSN
                };

                await session.StoreAsync(customerEntity);

                await session.SaveChangesAsync();
            }
        }
    }
}