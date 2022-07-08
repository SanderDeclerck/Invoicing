using System.Collections.Immutable;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceLineCollection
{
    public ImmutableList<InvoiceLine> Items { get; private set; } = ImmutableList<InvoiceLine>.Empty;

    public void AddLine(InvoiceLine invoiceLine)
    {
        Items = Items.Add(invoiceLine);
    }
}
