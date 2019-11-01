using System;
using System.Threading;
using EricBach.CQRS.Commands;
using MediatR;
using Pecuniary.Transaction.Data.Requests;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateTransactionCommand : Command, IRequest<CancellationToken>
    {
        public CreateTransactionRequest Transaction { get; set; }

        public CreateTransactionCommand(Guid id, CreateTransactionRequest transaction) : base(id)
        {
            Transaction = transaction;
        }
    }
}
