using System.Collections.Immutable;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceLineCollection
{
    public InvoiceLineCollection(IEnumerable<InvoiceLine> invoiceLines)
    {
        AddLines(invoiceLines);
    }

    public InvoiceLineCollection()
    {
    }

    public ImmutableList<InvoiceLine> Items { get; private set; } = ImmutableList<InvoiceLine>.Empty;

    public void AddLine(InvoiceLine invoiceLine)
    {
        Items = Items.Add(invoiceLine);
    }

    public void AddLines(IEnumerable<InvoiceLine> invoiceLines)
    {
        Items = Items.AddRange(invoiceLines);
    }
}
