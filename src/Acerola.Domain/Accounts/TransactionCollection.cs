﻿using Acerola.Domain.ValueObjects;

using System.Collections.ObjectModel;

namespace Acerola.Domain.Accounts
{
    public sealed class TransactionCollection
    {
        private readonly IList<ITransaction> _transactions;

        public TransactionCollection()
        {
            _transactions = new List<ITransaction>();
        }

        public IReadOnlyCollection<ITransaction> GetTransactions()
        {
            IReadOnlyCollection<ITransaction> transactions = new ReadOnlyCollection<ITransaction>(_transactions);
            return transactions;
        }

        public ITransaction GetLastTransaction()
        {
            ITransaction transaction = _transactions[_transactions.Count - 1];
            return transaction;
        }

        public void Add(ITransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void Add(IEnumerable<ITransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Add(transaction);
            }
        }

        public Amount GetCurrentBalance()
        {
            Amount totalAmount = 0;

            foreach (ITransaction transaction in _transactions)
            {
                if (transaction is Debit)
                    totalAmount = totalAmount - transaction.Amount;

                if (transaction is Credit)
                    totalAmount = totalAmount + transaction.Amount;
            }

            return totalAmount;
        }
    }
}