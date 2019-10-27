using System;
using Newtonsoft.Json;

namespace Pecuniary.Transaction.Data.ViewModels
{
    /// <summary>
    /// This has to match TransactionViewModel in Pecuniary.Queries
    /// </summary>
    public class TransactionViewModel
    {
        public Guid AccountId { get; set; }
        public SecurityViewModel Security { get; set; }
        public decimal Shares { get; set; }
        public decimal Price { get; set; }
        public decimal Commission { get; set; }
    }
}