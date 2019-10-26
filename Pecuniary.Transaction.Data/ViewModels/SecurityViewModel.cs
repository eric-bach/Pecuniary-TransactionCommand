using System;

namespace Pecuniary.Transaction.Data.ViewModels
{
    public class SecurityViewModel
    {
        public Guid SecurityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExchangeTypeCode { get; set; }
    }
}