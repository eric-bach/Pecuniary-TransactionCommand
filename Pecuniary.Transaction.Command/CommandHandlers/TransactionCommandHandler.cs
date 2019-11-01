using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
using _Transaction = Pecuniary.Transaction.Data.Models.Transaction;

namespace Pecuniary.Transaction.Command.CommandHandlers
{
    public class TransactionCommandHandlers : IRequestHandler<CreateTransactionCommand, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<_Transaction> _eventRepository;

        public TransactionCommandHandlers(IMediator mediator, IEventRepository<_Transaction> eventRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventRepository = eventRepository ?? throw new InvalidOperationException($"{nameof(_Transaction)}Repository is not initialized.");
        }

        public Task<CancellationToken> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            Logger.Log($"{nameof(CreateTransactionCommand)} handler invoked");

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            // Validate that the Account exists
            if (!_eventRepository.VerifyAggregateExists(command.Transaction.AccountId))
                throw new Exception($"{nameof(command.Transaction.AccountId)} {command.Transaction.AccountId} does not exist");
 
            // validate that the Security exists
            if (!_eventRepository.VerifyAggregateExists(command.Transaction.Security.SecurityId))
                throw new Exception($"{nameof(command.Transaction.Security)} with Id {command.Transaction.Security.SecurityId} does not exist");
            
            Logger.Log($"Initializing new {nameof(_Transaction)} aggregate {command.Id}");

            var transactionAggregate = new _Transaction(command.Id, command.Transaction);

            Logger.Log($"Saving new {nameof(_Transaction)} aggregate {command.Id}");

            // Save to Event Store
            _eventRepository.Save(transactionAggregate, transactionAggregate.Version);

            Logger.Log($"Completed saving {nameof(_Transaction)} aggregate {command.Id} to event store");

            return Task.FromResult(cancellationToken);
        }
    }
}
