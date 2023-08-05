using Acerola.Domain.Customers;

namespace Acerola.Application.Repositories
{
    public interface ICustomerWriteOnlyRepository
    {
        Task Add(Customer customer);

        Task Update(Customer customer);
    }
}