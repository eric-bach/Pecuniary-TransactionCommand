using System;
using System.Threading;
using EricBach.CQRS.Commands;
using MediatR;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateSecurityCommand : Command, IRequest<CancellationToken>
    {
        public TransactionViewModel Transaction { get; set; }
        public Guid TransactionId { get; set; }

        public CreateSecurityCommand(Guid id, Guid transactionId, TransactionViewModel transaction) : base(id)
        {
            if (string.IsNullOrEmpty(transaction.Security.Name))
                throw new Exception($"{nameof(transaction.Security.Name)} is required");
            if (string.IsNullOrEmpty(transaction.Security.Description))
                throw new Exception($"{nameof(transaction.Security.Description)} is required");
            if (string.IsNullOrEmpty(transaction.Security.ExchangeTypeCode) || (transaction.Security.ExchangeTypeCode != "TSX" && transaction.Security.ExchangeTypeCode != "NYSE" && transaction.Security.ExchangeTypeCode != "NASDAQ"))
                throw new Exception($"{nameof(transaction.Security.ExchangeTypeCode)} is invalid.  Must be one of [TSX, NYSE, NASDAQ]");

            Transaction = transaction;
            TransactionId = transactionId;
        }
    }
}
