using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
using _Transaction = Pecuniary.Transaction.Data.Models.Transaction;
using _Security = Pecuniary.Transaction.Data.Models.Security;

namespace Pecuniary.Transaction.Command.CommandHandlers
{
    public class AccountCommandHandlers : IRequestHandler<CreateTransactionCommand, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<_Transaction> _transactionRepository;
        private readonly IEventRepository<_Security> _securityRepository;

        public AccountCommandHandlers(IMediator mediator, IEventRepository<_Transaction> transactionRepository, IEventRepository<_Security> securityRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _transactionRepository = transactionRepository ?? throw new InvalidOperationException($"{nameof(_Transaction)}Repository is not initialized.");
            _securityRepository = securityRepository ?? throw new InvalidOperationException($"{nameof(_Transaction)}Repository is not initialized.");
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
                // TODO How to pass Transaction Guid in? Or use DynamoDB to trigger another command
                _mediator.Send(new CreateSecurityCommand(Guid.NewGuid(), command.Transaction.Security), CancellationToken.None);
            }
            else
            {
                // continue
                var transactionAggregate = new _Transaction(command.Id, command.Transaction);

                // Save to Event Store
                _transactionRepository.Save(transactionAggregate, transactionAggregate.Version);
            
                Logger.Log($"Completed saving {nameof(_Transaction)} aggregate to event store");
            }

            return Task.FromResult(cancellationToken);
        }
    }
}
