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
            Transaction = transaction;
        }
    }
}
