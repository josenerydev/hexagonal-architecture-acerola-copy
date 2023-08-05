using Acerola.Application.Results;

namespace Acerola.Application.Queries
{
    public interface ICustomersQueries
    {
        Task<CustomerResult> GetCustomer(Guid customerId);
    }
}