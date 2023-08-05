using Acerola.Application.Queries;
using Acerola.Application.Results;
using Acerola.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.GetCustomerDetails
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersQueries customersQueries;

        public CustomersController(ICustomersQueries customersQueries)
        {
            this.customersQueries = customersQueries;
        }

        [HttpGet("{customerId}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            CustomerResult customer = await customersQueries.GetCustomer(customerId);

            if (customer == null)
                return new NoContentResult();

            List<AccountDetailsModel> acounts = new List<AccountDetailsModel>();

            foreach (var account in customer.Accounts)
            {
                List<TransactionModel> transactions = new List<TransactionModel>();

                foreach (var item in account.Transactions)
                {
                    var transaction = new TransactionModel(
                        item.Amount,
                        item.Description,
                        item.TransactionDate);

                    transactions.Add(transaction);
                }

                acounts.Add(new AccountDetailsModel(account.AccountId, account.CurrentBalance, transactions));
            }

            CustomerDetailsModel model = new CustomerDetailsModel(
                customer.CustomerId,
                customer.Personnummer,
                customer.Name,
                acounts);

            return new OkObjectResult(model);
        }
    }
}