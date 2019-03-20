using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using ISO3166;

namespace Customers.Api.Application.Validators
{
    public class Iso3166CountryCodeValidator : PropertyValidator
    {
        public Iso3166CountryCodeValidator() : base("'{PropertyName}' must be known as an ISO3166-1 country code.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue is string value && Country.List.Any(country => country.ThreeLetterCode == value.ToUpper());
        }
    }
}
