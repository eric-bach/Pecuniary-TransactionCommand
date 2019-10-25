using System;
using System.Threading;
using EricBach.CQRS.Commands;
using MediatR;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateSecurityCommand : Command, IRequest<CancellationToken>
    {
        public SecurityViewModel Security { get; set; }

        public CreateSecurityCommand(Guid id, SecurityViewModel security) : base(id)
        {
            if (string.IsNullOrEmpty(security.Name))
                throw new Exception($"{nameof(security.Name)} is required");
            if (string.IsNullOrEmpty(security.Description))
                throw new Exception($"{nameof(security.Description)} is required");
            if (string.IsNullOrEmpty(security.ExchangeTypeCode) || (security.ExchangeTypeCode != "TSX" && security.ExchangeTypeCode != "NYSE" && security.ExchangeTypeCode != "NASDAQ"))
                throw new Exception($"{nameof(security.ExchangeTypeCode)} is invalid.  Must be one of [TSX, NYSE, NASDAQ]");

            Security = security;
        }
    }
}
