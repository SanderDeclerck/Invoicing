namespace Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;

public interface IUniqueInvoiceNumberSource
{
    public int GetNextInvoiceNumber();
}