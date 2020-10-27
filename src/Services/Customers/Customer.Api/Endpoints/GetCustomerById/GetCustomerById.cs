using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Invoicing.Customers.Domain.CustomerAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Customers.Api.EndPoints.GetCustomerById
{
    public class GetCustomerById : BaseAsyncEndpoint<string, GetCusomerByIdResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerById(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("/api/customer/{customerId}")]
        public override async Task<ActionResult<GetCusomerByIdResult>> HandleAsync([FromRoute] string customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(customerId, cancellationToken);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(MapToResult(customer));
        }

        private static GetCusomerByIdResult MapToResult(Customer customer) =>
            new GetCusomerByIdResult(customer.Id,
                              customer.Name,
                              customer.BillingAddress?.ToString() ?? string.Empty,
                              customer.IsCompany);
    }
}