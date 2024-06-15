using Invoicing.SharedKernel.Domain;

namespace InvoiceService.Domain.InvoiceIssuers;

public class IssuerAddress : ValueObject
{
    public IssuerAddress(string streetAndNumber, string postalCode, string city, string country)
    {
        StreetAndNumber = streetAndNumber;
        PostalCode = postalCode;
        City = city;
        Country = country;
    }

    public string StreetAndNumber { get; }
    public string PostalCode { get; }
    public string City { get; }
    public string Country { get; }
}
