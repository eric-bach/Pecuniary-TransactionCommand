using System;
using EricBach.CQRS.Aggregate;
using EricBach.CQRS.EventHandlers;
using Pecuniary.Transaction.Data.Events;
using Pecuniary.Transaction.Data.ViewModels;
using ISnapshot = EricBach.CQRS.EventStore.Snapshots.ISnapshot;
using Snapshot = EricBach.CQRS.EventStore.Snapshots.Snapshot;

namespace Pecuniary.Transaction.Data.Models
{
    public class Security : AggregateRoot, IEventHandler<SecurityCreatedEvent>, ISnapshot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExchangeTypeCode { get; set; }

        public Security()
        {
        }

        public Security(Guid id, SecurityViewModel vm)
        {
            var securityViewModel = new SecurityViewModel
            {
                SecurityId = id,
                Name = vm.Name,
                Description = vm.Description,
                ExchangeTypeCode = vm.ExchangeTypeCode
            };

            ApplyChange(new SecurityCreatedEvent(id, securityViewModel));
        }

        public void Handle(SecurityCreatedEvent e)
        {
            Id = e.Id;
            Name = e.Security.Name;
            Description = e.Security.Description;
            ExchangeTypeCode = e.Security.ExchangeTypeCode;
            
            Version = e.Version;
        }

        public Snapshot GetSnapshot()
        {
            return new SecuritySnapshot(Id, Name, Version);
        }

        public void SetSnapshot(Snapshot snapshot)
        {
            Name = ((SecuritySnapshot) snapshot).Name;

            Id = snapshot.Id;
            Version = snapshot.Version;
        }
    }
}
