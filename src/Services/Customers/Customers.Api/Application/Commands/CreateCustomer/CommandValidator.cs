using FluentValidation;

namespace Customers.Api.Application.Commands.CreateCustomer
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty();
            RuleFor(c => c.LastName).NotEmpty();
            RuleFor(c => c.EmailAddress).EmailAddress();
        }
    }
}
