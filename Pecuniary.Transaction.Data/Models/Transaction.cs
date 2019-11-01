using System;
using EricBach.CQRS.Aggregate;
using EricBach.CQRS.EventHandlers;
using Pecuniary.Transaction.Data.Events;
using Pecuniary.Transaction.Data.Requests;

namespace Pecuniary.Transaction.Data.Models
{
    public class Transaction : AggregateRoot, IEventHandler<TransactionCreatedEvent>
    {
        public Guid AccountId { get; set; }

        public Transaction()
        {
        }

        public Transaction(Guid id, CreateTransactionRequest vm)
        {
            ApplyChange(new TransactionCreatedEvent(id, vm));
        }

        public void Handle(TransactionCreatedEvent e)
        {
            Id = e.Id;
            AccountId = e.Transaction.AccountId;
            
            Version = e.Version;
            EventVersion = e.Version;
        }
    }
}
