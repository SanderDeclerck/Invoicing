using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Invoicing.Customers.Domain.CustomerAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Customers.Api.CustomerEndpoints.CreateCompany
{
    public class CreateCompany : BaseAsyncEndpoint<CreateCompanyCommand, CreateCompanyResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCompany(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost("/api/customer")]
        public override async Task<ActionResult<CreateCompanyResult>> HandleAsync(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company(request.CompanyName!, request.VatNumber!);

            await _customerRepository.AddAsync(company, cancellationToken);

            return new CreateCompanyResult(company.Id, company.Name, company.VatNumber);
        }
    }
}