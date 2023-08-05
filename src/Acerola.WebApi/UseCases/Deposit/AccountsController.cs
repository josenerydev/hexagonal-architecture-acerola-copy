using Acerola.Application.Commands.Deposit;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.Deposit
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IDepositUseCase depositService;

        public AccountsController(
            IDepositUseCase depositService)
        {
            this.depositService = depositService;
        }

        /// <summary>
        /// Deposit from an account
        /// </summary>
        [HttpPatch("Deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            DepositResult depositResult = await depositService.Execute(
                request.AccountId,
                request.Amount);

            if (depositResult == null)
            {
                return new NoContentResult();
            }

            Model model = new Model(
                depositResult.Transaction.Amount,
                depositResult.Transaction.Description,
                depositResult.Transaction.TransactionDate,
                depositResult.UpdatedBalance
            );

            return new ObjectResult(model);
        }
    }
}