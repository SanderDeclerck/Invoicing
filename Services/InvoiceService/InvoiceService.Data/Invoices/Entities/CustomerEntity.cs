namespace InvoiceService.Data.Invoices.Entities;

public class CustomerEntity
{
    public required string Name { get; init; }
    public required InvoiceAddressEntity Address { get; init; }
    public required VatNumberEntity? VatNumber { get; init; }
}
