using Acerola.Domain.ValueObjects;

namespace Acerola.Application.Commands.Deposit
{
    public interface IDepositUseCase
    {
        Task<DepositResult> Execute(Guid accountId, Amount amount);
    }
}