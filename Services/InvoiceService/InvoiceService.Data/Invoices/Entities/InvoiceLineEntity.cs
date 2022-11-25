namespace InvoiceService.Data.Invoices.Entities;

public class InvoiceLineEntity
{
    public required string Description { get; init; }
    public required decimal UnitPrice { get; init; }
    public required decimal Quantity { get; init; }
    public required int VatPercentage { get; init; }

}