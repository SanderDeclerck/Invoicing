using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceLine : ValueObject
{
    public InvoiceLine(string description, decimal unitPrice, decimal quantity, int vatPercentage)
    {
        Description = description;
        UnitPrice = unitPrice;
        Quantity = quantity;
        VatPercentage = vatPercentage;
    }

    public string Description { get; }
    public decimal UnitPrice { get; }
    public decimal Quantity { get; }
    public int VatPercentage { get; }

    public decimal TotalExcludingVat => UnitPrice * Quantity;
    public decimal VatAmount => TotalExcludingVat * VatPercentage / 100;
    public decimal TotalIncludingVat => TotalExcludingVat + VatAmount;
}
