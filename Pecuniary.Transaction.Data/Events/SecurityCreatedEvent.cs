using System;
using EricBach.CQRS.Events;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Events
{
    public class SecurityCreatedEvent : Event
    {
        private const int _eventVersion = 1;

        public SecurityViewModel Security { get; internal set; } = new SecurityViewModel();

        public SecurityCreatedEvent() : base(nameof(SecurityCreatedEvent), _eventVersion)
        {
        }

        public SecurityCreatedEvent(Guid id, SecurityViewModel security) : base(nameof(SecurityCreatedEvent), _eventVersion)
        {
            Id = id;
            EventName = nameof(SecurityCreatedEvent);

            Security = security;
        }
    }
}
