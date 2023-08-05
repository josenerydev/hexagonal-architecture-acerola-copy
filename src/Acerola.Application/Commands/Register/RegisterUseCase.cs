using Acerola.Application.Repositories;
using Acerola.Domain.Accounts;
using Acerola.Domain.Customers;

namespace Acerola.Application.Commands.Register
{
    public sealed class RegisterUseCase : IRegisterUseCase
    {
        private readonly ICustomerWriteOnlyRepository customerWriteOnlyRepository;
        private readonly IAccountWriteOnlyRepository accountWriteOnlyRepository;

        public RegisterUseCase(
            ICustomerWriteOnlyRepository customerWriteOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository)
        {
            this.customerWriteOnlyRepository = customerWriteOnlyRepository;
            this.accountWriteOnlyRepository = accountWriteOnlyRepository;
        }

        public async Task<RegisterResult> Execute(string pin, string name, double initialAmount)
        {
            Customer customer = new Customer(pin, name);

            Account account = new Account(customer.Id);
            account.Deposit(initialAmount);
            Credit credit = (Credit)account.GetLastTransaction();

            customer.Register(account.Id);

            await customerWriteOnlyRepository.Add(customer);
            await accountWriteOnlyRepository.Add(account, credit);

            RegisterResult result = new RegisterResult(customer, account);
            return result;
        }
    }
}