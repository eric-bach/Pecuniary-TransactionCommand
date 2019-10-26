using System;
using EricBach.CQRS.Aggregate;
using EricBach.CQRS.EventHandlers;
using Pecuniary.Transaction.Data.Events;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Models
{
    public class Transaction : AggregateRoot, IEventHandler<TransactionCreatedEvent>
    {
        public Guid AccountId { get; set; }

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

            Version = e.Version;
        }
    }
}
