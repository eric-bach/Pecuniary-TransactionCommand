using System;

namespace Pecuniary.Transaction.Data.Requests
{
    public class CreateTransactionRequest
    {
        public Guid AccountId { get; set; }
        public CreateSecurityRequest Security { get; set; }
        public decimal Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }
    }
}