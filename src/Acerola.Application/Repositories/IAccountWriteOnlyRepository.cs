using Acerola.Domain.Accounts;

namespace Acerola.Application.Repositories
{
    public interface IAccountWriteOnlyRepository
    {
        Task Add(Account account, Credit credit);

        Task Update(Account account, Credit credit);

        Task Update(Account account, Debit debit);

        Task Delete(Account account);
    }
}