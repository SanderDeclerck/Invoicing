using FluentValidation;

namespace Customers.Api.Application.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> IsIso3611CountryCode<T>(this IRuleBuilder<T, string> rulebuilder)
        {
            return rulebuilder.SetValidator(new Iso3166CountryCodeValidator());
        }
    }
}
