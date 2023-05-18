namespace Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources.Invoices;

public interface IInvoiceNumberSourceRepository
{
    Task<InvoiceNumberSource> GetOrCreate(CancellationToken cancellationToken);
    Task Update(InvoiceNumberSource invoiceNumberSource, CancellationToken cancellationToken);
}