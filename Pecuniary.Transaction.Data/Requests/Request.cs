using System;

namespace Pecuniary.Transaction.Data.Requests
{
    public class Request
    {
        public Guid Id { get; set; }

        public Request(Guid id)
        {
            Id = id;
        }
    }
}