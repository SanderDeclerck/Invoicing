using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.SharedKernel.Domain;
using NodaTime;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class Invoice : EntityBase, IAggregateRoot
{
    public static Invoice CreateForCustomer(Customer customer)
    {
        return new Invoice(customer, Guid.NewGuid());
    }

    private Invoice(Customer customer, Guid id) : base(id)
    {
        Customer = customer;
    }

    public int? InvoiceNumber { get; private set; }
    public LocalDate? InvoiceDate { get; private set; }
    public Customer Customer { get; }
    public InvoiceLineCollection InvoiceLines { get; } = new InvoiceLineCollection();


    public void Finalize(IUniqueInvoiceNumberSource invoiceNumberSource, ZonedClock clock)
    {
        InvoiceNumber = invoiceNumberSource.GetNextInvoiceNumber();
        InvoiceDate = clock.GetCurrentDate();
    }
}
