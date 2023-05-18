using Invoicing.Services.InvoiceService.Invoices.Domain;
using Invoicing.SharedKernel.Data;

namespace Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice?> GetById(Guid id, CancellationToken cancellationToken);
    Task Insert(Invoice invoice, CancellationToken cancellationToken);
    Task Update(Invoice invoice, CancellationToken cancellationToken);
}