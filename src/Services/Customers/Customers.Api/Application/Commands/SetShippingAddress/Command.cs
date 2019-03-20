using MediatR;

namespace Customers.Api.Application.Commands.SetShippingAddress
{
    public class Command : IRequest<CommandResult>
    {
        public int CustomerId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string IsoCountryCode { get; set; }
    }
}
