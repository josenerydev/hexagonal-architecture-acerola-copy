using Acerola.Domain.Customers;

namespace Acerola.Application.Repositories
{
    public interface ICustomerReadOnlyRepository
    {
        Task<Customer> Get(Guid id);
    }
}