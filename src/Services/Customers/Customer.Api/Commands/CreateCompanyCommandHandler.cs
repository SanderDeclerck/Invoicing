using System.Threading;
using System.Threading.Tasks;
using Invoicing.Customers.Domain.CustomerAggregate;
using MediatR;

namespace Invoicing.Customers.Api.Commands
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCompanyCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CreateCompanyResult> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new Company(request.CompanyName!, request.VatNumber!);

            await _customerRepository.AddAsync(company);

            return new CreateCompanyResult(company.Id, company.Name, company.VatNumber);
        }
    }
}