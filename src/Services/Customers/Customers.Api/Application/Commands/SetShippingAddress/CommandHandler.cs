using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Customers.Api.Application.Exceptions;
using Customers.Api.Application.Queries;
using Invoicing.Customers.Domain.CustomerAggregate;
using MediatR;

namespace Customers.Api.Application.Commands.SetShippingAddress
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
            var customer = await _customerRepository.GetAsync(request.CustomerId);

            if (customer == null)
            {
                throw new EntityNotFoundException(request.CustomerId);
            }

            customer.SetShippingAddress(new Address(request.Street, request.City, request.PostalCode, request.IsoCountryCode));

            var result = await _customerRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            if (result)
            {
                return new CommandResult
                {
                    IsSuccess = true,
                    Customer = _mapper.Map<CustomerViewModel>(customer)
                };
            }
            else
            {
                return new CommandResult { IsSuccess = false };
            }
        }
    }
}
