using System;
using System.Threading;
using Amazon.Lambda.Core;
using EricBach.CQRS.Commands;
using EricBach.CQRS.EventRepository;
using MediatR;
using Pecuniary.Transaction.Data.Models;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Data.Commands
{
    public class CreateTransactionCommand : Command, IRequest<CancellationToken>
    {
        public TransactionViewModel Transaction { get; set; }

        public CreateTransactionCommand(Guid id, IEventRepository<Account> accountRepository, TransactionViewModel transaction) : base(id)
        {
            if (transaction.AccountId == Guid.Empty)
                throw new Exception($"{nameof(transaction.AccountId)} is required");

            // Validate that AccountId exists
            var accountExists = accountRepository.VerifyAggregateExists(transaction.AccountId);
            if (!accountExists)
                throw new Exception($"{nameof(transaction.AccountId)} {transaction.AccountId} does not exist");
            LambdaLogger.Log($"Found Account matching {transaction.AccountId}");

            if (transaction.Price < 0)
                throw new Exception($"{nameof(transaction.Price)} must be greater than $0.00");

            Transaction = transaction;
        }
    }
}
