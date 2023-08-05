using Acerola.Domain.ValueObjects;

namespace Acerola.Application.Commands.Withdraw
{
    public interface IWithdrawUseCase
    {
        Task<WithdrawResult> Execute(Guid accountId, Amount amount);
    }
}