using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.ViewModels;
using _Security = Pecuniary.Transaction.Data.Models.Security;

namespace Pecuniary.Transaction.Command.CommandHandlers
{
    public class SecurityCommandHandlers : IRequestHandler<CreateSecurityCommand, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<_Security> _securityRepository;

        public SecurityCommandHandlers(IMediator mediator, IEventRepository<_Security> securityRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _securityRepository = securityRepository ?? throw new InvalidOperationException($"{nameof(_Security)}Repository is not initialized.");
        }

        public Task<CancellationToken> Handle(CreateSecurityCommand command, CancellationToken cancellationToken)
        {
            Logger.Log($"{nameof(CreateSecurityCommand)} handler invoked");

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            Logger.Log($"Initializing new {nameof(_Security)} aggregate {command.Id}");

            var aggregate = new _Security(command.Id, command.Security);

            Logger.Log($"Saving new {nameof(_Security)} aggregate {command.Id}");

            // Save to Event Store
            _securityRepository.Save(aggregate, aggregate.Version);

            Logger.Log($"Completed saving {nameof(_Security)} aggregate {command.Id} to event store");

            // Issue a CreateTransactionCommand to create the Transaction
            Logger.Log($"Creating Transaction {command.TransactionId}");
            var transaction = new TransactionViewModel
            {
                AccountId = command.AccountId,
                Security = new SecurityViewModel
                {
                    SecurityId = command.Id,
                    Name = command.Security.Name,
                    Description = command.Security.Description,
                    ExchangeTypeCode = command.Security.ExchangeTypeCode
                }
            };
            _mediator.Send(new CreateTransactionCommand(command.TransactionId, transaction), CancellationToken.None);
            
            Logger.Log($"Successfully created Transaction {command.TransactionId}");

            return Task.FromResult(cancellationToken);
        }
    }
}
