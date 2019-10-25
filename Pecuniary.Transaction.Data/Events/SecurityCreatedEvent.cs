using System;
using EricBach.CQRS.Events;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Events
{
    public class SecurityCreatedEvent : Event
    {
        public SecurityViewModel Security { get; internal set; } = new SecurityViewModel();

        public SecurityCreatedEvent() : base(nameof(SecurityCreatedEvent))
        {
        }

        public SecurityCreatedEvent(Guid id, SecurityViewModel security) : base(nameof(SecurityCreatedEvent))
        {
            Id = id;
            EventName = nameof(SecurityCreatedEvent);

            Security.SecurityId = Id;
            Security.Name = security.Name;
            Security.Description = security.Description;
            Security.ExchangeTypeCode = security.ExchangeTypeCode;
        }
    }
}
