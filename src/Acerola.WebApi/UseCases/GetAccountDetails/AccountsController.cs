using Acerola.Application.Queries;
using Acerola.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.GetAccountDetails
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsQueries accountsQueries;

        public AccountsController(
            IAccountsQueries accountsQueries)
        {
            this.accountsQueries = accountsQueries;
        }

        /// <summary>
        /// Get an account balance
        /// </summary>
        [HttpGet("{accountId}", Name = "GetAccount")]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var account = await accountsQueries.GetAccount(accountId);

            List<TransactionModel> transactions = new List<TransactionModel>();

            foreach (var item in account.Transactions)
            {
                var transaction = new TransactionModel(
                    item.Amount,
                    item.Description,
                    item.TransactionDate);

                transactions.Add(transaction);
            }

            return new ObjectResult(new AccountDetailsModel(
                account.AccountId,
                account.CurrentBalance,
                transactions));
        }
    }
}