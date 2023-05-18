using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceTotals : ValueObject
{
    public InvoiceTotals(InvoiceLineCollection lines)
    {
        decimal totalIncludingVat = 0, totalExcludingVat = 0, totalVat = 0;

        foreach (var line in lines.Items) 
        {
            totalIncludingVat += line.TotalIncludingVat;
            totalExcludingVat += line.TotalExcludingVat;
            totalVat += line.VatAmount;
        }

        TotalIncludingVat = totalIncludingVat;
        TotalExcludingVat = totalExcludingVat;
        TotalVat = totalVat;
    }

    public decimal TotalIncludingVat { get; }
    public decimal TotalExcludingVat { get; }
    public decimal TotalVat { get; }
}
