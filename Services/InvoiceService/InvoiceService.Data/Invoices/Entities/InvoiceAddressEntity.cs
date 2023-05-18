namespace InvoiceService.Data.Invoices.Entities;

public class InvoiceAddressEntity
{
    public required string StreetAndNumber { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
}
