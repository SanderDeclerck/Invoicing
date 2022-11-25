using Invoicing.Services.InvoiceService.Invoices.Domain;
using NodaTime;

namespace InvoiceService.Data.Invoices.Entities;

public class InvoiceEntity 
{
    public required Guid Id { get; init; }
    public required int? InvoiceNumber { get; init; }
    public required LocalDate? InvoiceDate { get; init; }
    public required CustomerEntity Customer { get; init; }
    public required List<InvoiceLineEntity> InvoiceLines { get; init; } = new List<InvoiceLineEntity>();
}
