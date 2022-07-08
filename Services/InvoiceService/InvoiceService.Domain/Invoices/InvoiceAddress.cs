using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceAddress : ValueObject
{
    public InvoiceAddress(string streetAndNumber, string postalCode, string city, string country)
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
