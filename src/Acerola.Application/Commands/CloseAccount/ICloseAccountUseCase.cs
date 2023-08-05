namespace Acerola.Application.Commands.CloseAccount
{
    public interface ICloseAccountUseCase
    {
        Task<Guid> Execute(Guid accountId);
    }
}