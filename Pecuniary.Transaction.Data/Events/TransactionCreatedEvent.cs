using System;
using EricBach.CQRS.Events;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.Requests;

namespace Pecuniary.Transaction.Data.Events
{
    public class TransactionCreatedEvent : Event
    {
        private const int _eventVersion = 1;
        
        public CreateTransaction Transaction { get; internal set; }

        public TransactionCreatedEvent() : base(nameof(TransactionCreatedEvent), _eventVersion)
        {
        }

        public TransactionCreatedEvent(Guid id, CreateTransaction transaction) : base(nameof(TransactionCreatedEvent), _eventVersion)
        {
            Id = id;
            EventName = nameof(TransactionCreatedEvent);

            Transaction = transaction;
        }
    }
}
