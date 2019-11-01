using System;
using System.Threading;
using System.Threading.Tasks;
using EricBach.CQRS.EventRepository;
using MediatR;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.Requests;

namespace Pecuniary.Transaction.Command.RequestHandlers
{
    public class TransactionRequestHandler : IRequestHandler<CreateTransactionRequest, CancellationToken>
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<Data.Models.Transaction> _eventRepository;

        public TransactionRequestHandler(IMediator mediator, IEventRepository<Data.Models.Transaction> eventRepository)
        {
            _mediator = mediator;
            _eventRepository = eventRepository;
        }

        public Task<CancellationToken> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            if (request.Price < 0)
                throw new Exception($"{nameof(request.Price)} must be greater than $0.00");

            if (request.AccountId == Guid.Empty)
                throw new Exception($"{nameof(request.AccountId)} is required");

            if (request.Security.SecurityId == Guid.Empty)
                throw new Exception($"{nameof(request.Security.SecurityId)} is required");

            // TODO Use AutoMapper
            var createAccount = new CreateTransaction
            {
                AccountId = request.AccountId,
                Security = new CreateSecurity
                {
                    SecurityId = request.Security.SecurityId,
                    Name = request.Security.Name,
                    Description = request.Security.Description,
                    ExchangeTypeCode = request.Security.ExchangeTypeCode
                },
                Shares = request.Shares,
                Price = request.Price,
                Commission = request.Commission
            };

            // TODO Create Security Asynchronously - Lambda  or APIGW (too slow)
            //var task = _mediator.Send(new CreateSecurityCommand(request.Id, createAccount), CancellationToken.None);
            //task.Wait();
            //var result = task.Result;

            _mediator.Send(new CreateTransactionCommand(request.Id, createAccount), CancellationToken.None);

            return Task.FromResult(cancellationToken);
        }
    }
}
