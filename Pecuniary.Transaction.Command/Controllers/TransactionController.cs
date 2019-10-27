using System;
using EricBach.CQRS.Commands;
using EricBach.CQRS.EventRepository;
using EricBach.LambdaLogger;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.Models;
using Pecuniary.Transaction.Data.ViewModels;

namespace Pecuniary.Transaction.Command.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEventRepository<Account> AccountRepository;

        public TransactionController(IMediator mediator, IEventRepository<Account> accountRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            AccountRepository = accountRepository ?? throw new InvalidOperationException($"{nameof(Account)}Repository is not initialized.");
        }

        // POST api/transaction
        [HttpPost]
        public ActionResult<CommandResponse> Post([FromBody] TransactionViewModel vm)
        {
            Logger.Log($"Received {nameof(TransactionViewModel)}");

            var id = Guid.NewGuid();

            Logger.Log($"Created new Transaction Id: {id}");

            try
            {
                _mediator.Send(new CreateTransactionCommand(id, AccountRepository, vm));
            }
            catch (Exception e)
            {
                return BadRequest(new CommandResponse { Error = e.Message });
            }

            Logger.Log($"Completed processing {nameof(CreateTransactionCommand)}");

            return Ok(new CommandResponse {Id = id, Name = nameof(CreateTransactionCommand) });
        }
    }
}
