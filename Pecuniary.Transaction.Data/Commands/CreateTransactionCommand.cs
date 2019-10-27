using System;
using System.Threading;
using EricBach.CQRS.Commands;
using MediatR;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateTransactionCommand : Command, IRequest<CancellationToken>
    {
        public TransactionViewModel Transaction { get; set; }

        public CreateTransactionCommand(Guid id, TransactionViewModel transaction) : base(id)
        {
            if (transaction.AccountId == Guid.Empty)
                throw new Exception($"{nameof(transaction.AccountId)} is required");
            if (transaction.Price < 0)
                throw new Exception($"{nameof(transaction.Price)} must be greater than $0.00");

            Transaction = transaction;
        }
    }
}
