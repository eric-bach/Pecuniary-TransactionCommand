using System;
using System.Threading;
using EricBach.CQRS.Commands;
using MediatR;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateTransactionCommand : Command, IRequest<CancellationToken>
    {
        public CreateTransaction Transaction { get; set; }

        public CreateTransactionCommand(Guid id, CreateTransaction transaction) : base(id)
        {
            Transaction = transaction;
        }
    }

    public class CreateTransaction
    {
        public Guid AccountId { get; set; }
        public CreateSecurity Security { get; set; }
        public decimal Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }
    }

    public class CreateSecurity
    {
        public Guid SecurityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExchangeTypeCode { get; set; }
    }
}
