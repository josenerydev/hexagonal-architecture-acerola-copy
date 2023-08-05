namespace Acerola.Application.Commands.Register
{
    public interface IRegisterUseCase
    {
        Task<RegisterResult> Execute(string pin, string name, double initialAmount);
    }
}