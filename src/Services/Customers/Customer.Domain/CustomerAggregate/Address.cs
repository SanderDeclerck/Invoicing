using System;
using System.Collections.Generic;
using System.Linq;
using Invoicing.Base.Ddd;
using ISO3166;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string IsoCountryCode { get; private set; }
        public Country? Country => Country.List.FirstOrDefault(c => c.ThreeLetterCode == IsoCountryCode);

        public Address(string street, string city, string postalCode, string isoCountryCode)
        {
            isoCountryCode = isoCountryCode.ToUpper();

            if (!Country.List.Any(c => c.ThreeLetterCode == isoCountryCode))
            {
                throw new ArgumentException($"Unknown ISO3611-1 three digit country code {isoCountryCode}.", nameof(isoCountryCode));
            }

            Street = street;
            City = city;
            PostalCode = postalCode;
            IsoCountryCode = isoCountryCode;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return City;
            yield return PostalCode;
            yield return IsoCountryCode;
        }
    }
}
