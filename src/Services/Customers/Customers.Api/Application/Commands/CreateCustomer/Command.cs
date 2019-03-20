using MediatR;

namespace Customers.Api.Application.Commands.CreateCustomer
{
    public class Command : IRequest<CommandResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
