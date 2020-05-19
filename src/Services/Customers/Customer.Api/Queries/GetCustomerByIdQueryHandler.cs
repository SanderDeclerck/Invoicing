using System.Threading;
using System.Threading.Tasks;
using Invoicing.Customers.Domain.CustomerAggregate;
using MediatR;

namespace Invoicing.Customers.Api.Queries
{

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetCustomerByIdResult> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(request.CustomerId);

            if (customer == null)
            {
                return GetCustomerByIdResult.NotFound(request.CustomerId);
            }

            return new GetCustomerByIdResult(customer.Id,
                customer.Name,
                customer.BillingAddress?.ToString() ?? string.Empty,
                customer.IsCompany);
        }
    }
}