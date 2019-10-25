using System;
using EricBach.CQRS.EventStore.Snapshots;

namespace Pecuniary.Transaction.Data.Models
{
    public class TransactionSnapshot : Snapshot
    {
        public int EventVersion { get; set; }

        public TransactionSnapshot()
        {
        }

        public TransactionSnapshot(Guid id, int eventVersion)
        {
            Id = id;
            EventVersion = eventVersion;
        }
    }
}
