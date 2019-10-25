using System;

namespace Pecuniary.Transaction.Data.Responses
{
    public class CommandResponse
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string Error { get; set; }
    }
}
