using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Invoicing.Customers.Domain.CustomerAggregate;
using Microsoft.AspNetCore.Mvc;


namespace Invoicing.Customers.Api.Endpoints.GetCustomerList
{
    public class GetCustomerList : BaseAsyncEndpoint<GetCustomerListResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerList(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("/api/customers")]
        public override async Task<ActionResult<GetCustomerListResult>> HandleAsync(CancellationToken cancellationToken)
        {
            var customerList = await _customerRepository.GetList(cancellationToken);
            return Ok(new GetCustomerListResult(customerList.Select(MapToCustomer)));
        }

        private static GetCustomerListResult.Customer MapToCustomer(Customer customer) =>
            new GetCustomerListResult.Customer(customer.Id,
                                               customer.Name,
                                               customer.BillingAddress?.ToString() ?? string.Empty,
                                               customer.IsCompany);

    }
}