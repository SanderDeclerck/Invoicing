using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class VatTotal : ValueObject
{
    public VatTotal(int vatPercentage, decimal totalExcludingVat, decimal vatAmount)
    {
        VatPercentage = vatPercentage;
        TotalExcludingVat = totalExcludingVat;
        VatAmount = vatAmount;
    }
    
    public int VatPercentage { get; }
    public decimal TotalExcludingVat { get; }
    public decimal VatAmount { get; }
}