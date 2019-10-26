using System;
using EricBach.CQRS.Events;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Events
{
    public class TransactionCreatedEvent : Event
    {
        private const int _eventVersion = 1;
        
        public TransactionViewModel Transaction { get; internal set; } = new TransactionViewModel();

        public TransactionCreatedEvent() : base(nameof(TransactionCreatedEvent), _eventVersion)
        {
        }

        public TransactionCreatedEvent(Guid id, TransactionViewModel transaction) : base(nameof(TransactionCreatedEvent), _eventVersion)
        {
            Id = id;
            EventName = nameof(TransactionCreatedEvent);

            Transaction = transaction;
        }
    }
}
