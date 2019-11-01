using System;
using EricBach.CQRS.Commands;
using EricBach.LambdaLogger;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pecuniary.Transaction.Data.Requests;

namespace Pecuniary.Transaction.Command.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // POST api/transaction
        [HttpPost]
        public ActionResult<CommandResponse> Post([FromBody] CreateTransactionRequest vm)
        {
            Logger.Log($"Received {nameof(CreateTransactionRequest)}");

            var id = Guid.NewGuid();

            try
            {
                _mediator.Send(new CreateTransactionRequest(id, vm.AccountId, vm.Security, vm.Shares, vm.Price, vm.Commission));
            }
            catch (Exception e)
            {
                return BadRequest(new CommandResponse { Error = e.Message });
            }

            Logger.Log($"Completed processing {nameof(CreateTransactionRequest)}");

            return Ok(new CommandResponse {Id = id, Name = nameof(CreateTransactionRequest) });
        }
    }
}
