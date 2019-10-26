using System;
using EricBach.CQRS.Events;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Events
{
    public class TransactionCreatedEvent : Event
    {
        public TransactionViewModel Transaction { get; internal set; } = new TransactionViewModel();

        public TransactionCreatedEvent() : base(nameof(TransactionCreatedEvent))
        {
        }

        public TransactionCreatedEvent(Guid id, TransactionViewModel transaction) : base(nameof(TransactionCreatedEvent))
        {
            Id = id;
            EventName = nameof(TransactionCreatedEvent);

            Transaction.AccountId = transaction.AccountId;
            Transaction.Security = transaction.Security;
        }
    }
}
