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

        public Transaction(Guid id, CreateTransactionRequest request)
        {
            ApplyChange(new TransactionCreatedEvent(id, request));
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
