using System;
using EricBach.LambdaLogger;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pecuniary.Transaction.Data.Commands;
using Pecuniary.Transaction.Data.Responses;
using Pecuniary.Transaction.Data.ViewModels;

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
        public ActionResult<CommandResponse> Post([FromBody] TransactionViewModel vm)
        {
            Logger.Log($"Received {nameof(TransactionViewModel)}");

            var id = Guid.NewGuid();
            
            try
            {
                _mediator.Send(new CreateTransactionCommand(id, vm));
            }
            catch (Exception e)
            {
                return BadRequest(new CommandResponse { Id = id, Error= e.Message});
            }

            Logger.Log($"Completed processing {nameof(CreateTransactionCommand)}");

            return Ok(new CommandResponse {Id = id, Name = nameof(CreateTransactionCommand) });
        }
    }
}
