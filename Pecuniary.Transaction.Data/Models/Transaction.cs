using System;
using EricBach.CQRS.Aggregate;
using EricBach.CQRS.EventHandlers;
using Pecuniary.Transaction.Data.Events;
using Pecuniary.Transaction.Data.ViewModels;
using ISnapshot = EricBach.CQRS.EventStore.Snapshots.ISnapshot;
using Snapshot = EricBach.CQRS.EventStore.Snapshots.Snapshot;

namespace Pecuniary.Transaction.Data.Models
{
    public class Transaction : AggregateRoot, IEventHandler<TransactionCreatedEvent>, ISnapshot
    {
        public Guid AccountId { get; set; }
        public Security Security { get; set; }

        public Transaction()
        {
        }

        public Transaction(Guid id, TransactionViewModel vm)
        {
            var transactionViewModel = new TransactionViewModel
            {
                AccountId = vm.AccountId,
                Security = vm.Security
            };

            ApplyChange(new TransactionCreatedEvent(id, transactionViewModel));
        }

        public void Handle(TransactionCreatedEvent e)
        {
            Id = e.Id;
            AccountId = e.Transaction.AccountId;
            Security.Name = e.Transaction.Security.Name;
            Security.Description = e.Transaction.Security.Description;
            Security.ExchangeTypeCode = e.Transaction.Security.ExchangeTypeCode;

            Version = e.Version;
        }

        public Snapshot GetSnapshot()
        {
            return new TransactionSnapshot(Id, Version);
        }

        public void SetSnapshot(Snapshot snapshot)
        {
            //AccountId = ((TransactionSnapshot) snapshot).AccountId;

            Id = snapshot.Id;
            Version = snapshot.Version;
        }
    }
}
