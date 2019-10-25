using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
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

            var aggregate = new _Security(command.Id, command.Security);

            // Save to Event Store
            _securityRepository.Save(aggregate, aggregate.Version);

            Logger.Log($"Completed saving {nameof(_Security)} aggregate to event store");

            return Task.FromResult(cancellationToken);
        }
    }
}
