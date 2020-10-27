using FluentValidation;

namespace Invoicing.Customers.Api.Endpoints.CreateCompany
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