using System;
using EricBach.CQRS.Requests;

namespace Pecuniary.Transaction.Data.Requests
{
    public class CreateTransactionRequest : Request
    {
        public Guid AccountId { get; set; }
        public CreateSecurityRequest Security { get; set; }
        public decimal Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }

        public CreateTransactionRequest(Guid id) : base(id)
        {
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