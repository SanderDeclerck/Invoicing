using MediatR;

namespace Invoicing.Customers.Api.Commands
{
    public class CreateCompanyCommand : IRequest<CreateCompanyResult>
    {
        public string? CompanyName { get; set; }

        public string? VatNumber { get; set; }
    }
}