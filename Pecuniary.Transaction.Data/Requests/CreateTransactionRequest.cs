using System;
using System.Threading;
using MediatR;

namespace Pecuniary.Transaction.Data.Requests
{
    public class CreateTransactionRequest : Request, IRequest<CancellationToken>
    {
        public Guid AccountId { get; set; }
        public CreateSecurityRequest Security { get; set; }
        public decimal Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }

        public CreateTransactionRequest(Guid id, Guid accountId, CreateSecurityRequest security, decimal shares, decimal price, decimal commission) : base(id)
        {
            AccountId = accountId;
            Security = security;
            Shares = shares;
            Price = price;
            Commission = commission;
        }
    }

    public class CreateSecurityRequest
    {
        public Guid SecurityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExchangeTypeCode { get; set; }
    }
}