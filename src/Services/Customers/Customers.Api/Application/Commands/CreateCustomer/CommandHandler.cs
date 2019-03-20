using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Customers.Api.Application.Queries;
using Invoicing.Customers.Domain.CustomerAggregate;
using MediatR;

namespace Customers.Api.Application.Commands.CreateCustomer
{
    public class CommandHandler : IRequestHandler<Command, CommandResult>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer(request.FirstName, request.LastName, request.CompanyName, request.VatNumber,
                request.PhoneNumber, request.EmailAddress);

            _customerRepository.Add(newCustomer);

            var result = await _customerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return new CommandResult
            {
                IsSuccess = result,
                Customer = result ? _mapper.Map<CustomerViewModel>(newCustomer) : null
            };
        }
    }
}
