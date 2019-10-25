using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Newtonsoft.Json;
using Pecuniary.Transaction.Data.Commands;
using _Transaction = Pecuniary.Transaction.Data.Models.Transaction;
using _Security = Pecuniary.Transaction.Data.Models.Security;

namespace Pecuniary.Transaction.Command.CommandHandlers
{
    public class TransactionCommandHandlers : IRequestHandler<CreateTransactionCommand, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<_Transaction> _transactionRepository;
        private readonly IEventRepository<_Security> _securityRepository;

        public TransactionCommandHandlers(IMediator mediator, IEventRepository<_Transaction> transactionRepository, IEventRepository<_Security> securityRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _transactionRepository = transactionRepository ?? throw new InvalidOperationException($"{nameof(_Transaction)}Repository is not initialized.");
            _securityRepository = securityRepository ?? throw new InvalidOperationException($"{nameof(_Security)}Repository is not initialized.");
        }

        public Task<CancellationToken> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            Logger.Log($"{nameof(CreateTransactionCommand)} handler invoked");

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var securityId = command.Transaction.Security.SecurityId;
            var securityAggregate = securityId != Guid.Empty ? _securityRepository.GetById(securityId) : null;
            if (securityAggregate == null)
            {
                // CreateSecurityCommand
                Logger.Log($"Security {command.Transaction.Security.Name} does not exist. Creating...");
                
                // TODO How to pass Transaction Guid in? Or use DynamoDB to trigger another command
                securityId = Guid.NewGuid();
                _mediator.Send(new CreateSecurityCommand(securityId, command.Transaction.Security, command.Transaction.AccountId, Guid.NewGuid()), CancellationToken.None);
                
                Logger.Log($"Successfully created Security {securityId}");
            }
            else
            {
                Logger.Log($"Initializing new {nameof(_Transaction)} aggregate {command.Id}");

                // CreateTransactionCommand
                var transactionAggregate = new _Transaction(command.Id, command.Transaction);

                Logger.Log($"Saving new {nameof(_Transaction)} aggregate {command.Id}");

                // Save to Event Store
                _transactionRepository.Save(transactionAggregate, transactionAggregate.Version);

                Logger.Log($"Completed saving {nameof(_Transaction)} aggregate {command.Id} to event store");
            }

            return Task.FromResult(cancellationToken);
        }
    }
}
