using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Customers.Api.Application.Exceptions;
using Customers.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerQueries _customerQueries;
        private readonly IMediator _mediator;

        public CustomersController(ICustomerQueries customerQueries, IMediator mediator)
        {
            _customerQueries = customerQueries;
            _mediator = mediator;
        }

        [HttpGet("{customerId:int}")]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetCustomerAsync(int customerId)
        {
            try
            {
                var customer = _customerQueries.GetCustomerById(customerId);
            
                return Ok(customer);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerViewModelSlim>), (int)HttpStatusCode.OK)]
        public IActionResult GetCustomerListAsync()
        {
            return Ok(_customerQueries.GetAllCustomers());
        }

        [HttpPut("SetBillingAddress")]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetBillingAddress([FromBody] Application.Commands.SetBillingAddress.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return result.IsSuccess ? (IActionResult)Ok(result.Customer) : BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("SetShippingAddress")]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetShippingAddress([FromBody] Application.Commands.SetShippingAddress.Command command)
        {
            try
            {
                var result = await _mediator.Send(command);

                return result.IsSuccess ? (IActionResult)Ok(result.Customer) : BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] Application.Commands.CreateCustomer.Command command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ? (IActionResult)Ok(result.Customer) : BadRequest();
        }
    }
}
