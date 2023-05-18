using System.Collections.Immutable;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class InvoiceLineCollection
{
    private bool _isFrozen = false;

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
        if (_isFrozen)
        {
            throw new InvalidOperationException("Invoice is finalized and cannot be modified");
        }

        Items = Items.Add(invoiceLine);
    }

    public void AddLines(IEnumerable<InvoiceLine> invoiceLines)
    {
        if (_isFrozen)
        {
            throw new InvalidOperationException("Invoice is finalized and cannot be modified");
        }

        Items = Items.AddRange(invoiceLines);
    }

    internal void Freeze()
    {
        _isFrozen = true;
    }
}
