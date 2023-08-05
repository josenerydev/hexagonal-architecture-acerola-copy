﻿using Acerola.Application.Commands.Withdraw;

using Microsoft.AspNetCore.Mvc;

namespace Acerola.WebApi.UseCases.Withdraw
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IWithdrawUseCase withdrawService;

        public AccountsController(IWithdrawUseCase withdrawService)
        {
            this.withdrawService = withdrawService;
        }

        /// <summary>
        /// Withdraw from an account
        /// </summary>
        [HttpPatch("Withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            WithdrawResult depositResult = await withdrawService.Execute(
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