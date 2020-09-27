using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Invoicing.Customers.Domain.CustomerAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Customers.Api.CustomerEndpoints.GetById
{
    public class GetById : BaseAsyncEndpoint<string, GetByIdResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetById(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("/api/customer/{customerId}")]
        public override async Task<ActionResult<GetByIdResult>> HandleAsync([FromRoute] string customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(customerId, cancellationToken);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(MapToResult(customer));
        }

        private static GetByIdResult MapToResult(Customer customer) =>
            new GetByIdResult(customer.Id,
                              customer.Name,
                              customer.BillingAddress?.ToString() ?? string.Empty,
                              customer.IsCompany);
    }
}