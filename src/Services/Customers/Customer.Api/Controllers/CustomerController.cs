using System;
using System.Threading;
using System.Threading.Tasks;
using Invoicing.Customers.Api.Commands;
using Invoicing.Customers.Api.Queries;
using Invoicing.Customers.Domain.CustomerAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Customer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(ICustomerRepository customerRepository, IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetByIdAsync(string customerId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(customerId), cancellationToken);

            if (!result.IsFound)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("page/{page}/pageSize/{pagesize}")]
        public Task<IActionResult> GetListAsync(int page, int pagesize)
        {
            throw new NotImplementedException();
        }

        [HttpPost("company")]
        public async Task<IActionResult> CreateCompanyAsync(CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
