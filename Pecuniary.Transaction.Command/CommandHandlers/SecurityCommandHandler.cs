using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.Models;

namespace Pecuniary.Transaction.Command.CommandHandlers
{
    public class SecurityCommandHandlers : IRequestHandler<CreateSecurityCommand, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<Security> SecurityRepository;
        private readonly IEventRepository<Account> AccountRepository;

        public SecurityCommandHandlers(IMediator mediator, IEventRepository<Security> securityRepository, IEventRepository<Account> accountRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            SecurityRepository = securityRepository ?? throw new InvalidOperationException($"{nameof(Security)}Repository is not initialized.");
            AccountRepository = accountRepository ?? throw new InvalidOperationException($"{nameof(Account)}Repository is not initialized.");
        }

        public Task<CancellationToken> Handle(CreateSecurityCommand command, CancellationToken cancellationToken)
        {
            Logger.Log($"{nameof(CreateSecurityCommand)} handler invoked");

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            Logger.Log($"Initializing new {nameof(Security)} aggregate {command.Id}");

            var aggregate = new Security(command.Id, command.Transaction.Security);

            Logger.Log($"Saving new {nameof(Security)} aggregate {command.Id}");

            // Save to Event Store
            SecurityRepository.Save(aggregate, aggregate.Version);
            
            Logger.Log($"Completed saving {nameof(Security)} aggregate {command.Id} to event store");

            // Issue a CreateTransactionCommand to create the Transaction
            Logger.Log($"Creating Transaction {command.TransactionId}");
            var transaction = command.Transaction;
            // Update the Security Id now that a Security has been created
            transaction.Security.SecurityId = command.Id;
            _mediator.Send(new CreateTransactionCommand(command.TransactionId, AccountRepository, transaction), CancellationToken.None);
            
            Logger.Log($"Successfully created Transaction {command.TransactionId}");

            return Task.FromResult(cancellationToken);
        }
    }
}
