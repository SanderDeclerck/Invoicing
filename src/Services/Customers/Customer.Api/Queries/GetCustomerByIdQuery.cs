using MediatR;

namespace Invoicing.Customers.Api.Queries
{
    public class GetCustomerByIdQuery : IRequest<GetCustomerByIdResult>
    {
        public string CustomerId { get; }

        public GetCustomerByIdQuery(string customerId)
        {
            CustomerId = customerId;
        }
    }
}