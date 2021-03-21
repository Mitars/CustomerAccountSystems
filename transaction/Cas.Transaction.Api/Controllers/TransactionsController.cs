using Cas.Transaction.Core;
using Cas.Transaction.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Cas.Transaction.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TransactionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDetailsDto>> Get(long id) =>
            await this.mediator.Send(new Details.Query { Id = id });

        [HttpGet]
        public async Task<ActionResult<List<TransactionDetailsDto>>> GetByAccountId([Required, FromQuery] long accountId) =>
            await this.mediator.Send(new List.Query { AccountId = accountId });

        [HttpPost]
        public async Task<ActionResult<TransactionDetailsDto>> Create(Create.Command command) =>
            await this.mediator.Send(command);
    }
}
