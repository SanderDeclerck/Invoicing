using FluentValidation;

namespace Invoicing.Customers.Api.Commands
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(command => command.CompanyName).NotEmpty();
            RuleFor(command => command.VatNumber).NotEmpty();
        }
    }
}