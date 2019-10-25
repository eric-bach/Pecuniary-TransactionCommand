using System;
using EricBach.CQRS.EventStore.Snapshots;

namespace Pecuniary.Transaction.Data.Models
{
    public class SecuritySnapshot : Snapshot
    {
        public string Name { get; set; }
        public int EventVersion { get; set; }

        public SecuritySnapshot()
        {
        }

        public SecuritySnapshot(Guid id, string name, int eventVersion)
        {
            Id = id;
            Name = name;
            EventVersion = eventVersion;
        }
    }
}
