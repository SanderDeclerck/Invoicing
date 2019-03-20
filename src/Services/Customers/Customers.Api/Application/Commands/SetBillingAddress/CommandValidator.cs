using Customers.Api.Application.Validators;
using FluentValidation;

namespace Customers.Api.Application.Commands.SetBillingAddress
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
            RuleFor(c => c.Street).NotEmpty();
            RuleFor(c => c.City).NotEmpty();
            RuleFor(c => c.PostalCode).NotEmpty();
            RuleFor(c => c.IsoCountryCode).Length(3).IsIso3611CountryCode();
        }
    }
}
