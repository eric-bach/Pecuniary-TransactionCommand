using EricBach.CQRS.Aggregate;

namespace Pecuniary.Transaction.Data.Models
{
    public class Account : AggregateRoot
    {
        public string Name { get; set; }
        public string AccountTypeCode { get; set; }
    }
}