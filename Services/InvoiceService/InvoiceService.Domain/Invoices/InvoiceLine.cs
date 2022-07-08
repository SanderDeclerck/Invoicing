using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceLine : ValueObject
{
    public InvoiceLine(string description, decimal unitPrice, decimal amount, int vatPercentage)
    {
        Description = description;
        UnitPrice = unitPrice;
        Amount = amount;
        VatPercentage = vatPercentage;
    }

    public string Description { get; }
    public decimal UnitPrice { get; }
    public decimal Amount { get; }
    public int VatPercentage { get; }

    public decimal TotalExcludingVat => UnitPrice * Amount;
    public decimal VatAmount => TotalExcludingVat * VatPercentage / 100;
    public decimal TotalIncludingVat => TotalExcludingVat + VatAmount;
}
