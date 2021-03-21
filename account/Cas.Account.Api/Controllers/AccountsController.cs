using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Cas.Account.Core;
using Cas.Account.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cas.Account.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AccountsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetailsDto>> Get(long id) =>
            await this.mediator.Send(new Details.Query { Id = id });

        [HttpGet]
        public async Task<ActionResult<List<AccountDetailsDto>>> GetByCustomerId([Required, FromQuery] long customerId) =>
            await this.mediator.Send(new List.Query { CustomerId = customerId });

        [HttpPost]
        public async Task<ActionResult<AccountDetailsDto>> Create(Create.Command command) =>
            await this.mediator.Send(command);
    }
}
