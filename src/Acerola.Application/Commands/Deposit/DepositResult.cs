﻿using Acerola.Application.Results;
using Acerola.Domain.Accounts;
using Acerola.Domain.ValueObjects;

namespace Acerola.Application.Commands.Deposit
{
    public class DepositResult
    {
        public TransactionResult Transaction { get; }
        public double UpdatedBalance { get; }

        public DepositResult(
            Credit credit,
            Amount updatedBalance)
        {
            Transaction = new TransactionResult(
                credit.Description,
                credit.Amount,
                credit.TransactionDate);

            UpdatedBalance = updatedBalance;
        }
    }
}