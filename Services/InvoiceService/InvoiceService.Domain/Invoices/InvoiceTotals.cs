using System.Collections.Immutable;
using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceTotals : ValueObject
{
    public InvoiceTotals(InvoiceLineCollection lines)
    {
        TotalExcludingVat = lines.Items.Sum(line => line.TotalExcludingVat);
        TotalVat = lines.Items.Sum(line => line.VatAmount);
        TotalIncludingVat = TotalExcludingVat + TotalVat;
        VatTotals = ImmutableList<VatTotal>.Empty.AddRange(
            lines.Items
                .GroupBy(line => line.VatPercentage)
                .Select(group => new VatTotal(group.Key, group.Sum(line => line.TotalExcludingVat), group.Sum(line => line.VatAmount))));
    }

    public decimal TotalIncludingVat { get; }
    public decimal TotalExcludingVat { get; }
    public decimal TotalVat { get; }
    public ImmutableList<VatTotal> VatTotals { get; }
}
