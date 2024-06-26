using System.Diagnostics.CodeAnalysis;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.SharedKernel.Domain;
using NodaTime;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class Invoice : EntityBase, IAggregateRoot
{
    public static Invoice CreateInvoiceForCustomer(Customer customer)
    {
        return new Invoice(customer, Guid.NewGuid());
    }

    public Invoice(Guid id, int? invoiceNumber, LocalDate? invoiceDate, Customer customer, IEnumerable<InvoiceLine> invoiceLines) : base(id)
    {
        InvoiceNumber = invoiceNumber;
        InvoiceDate = invoiceDate;
        Customer = customer;
        InvoiceLines = new InvoiceLineCollection(invoiceLines);
        SetFinalized();
    }

    private Invoice(Customer customer, Guid id) : base(id)
    {
        Customer = customer;
    }

    public int? InvoiceNumber { get; private set; }
    public LocalDate? InvoiceDate { get; private set; }
    public Customer Customer { get; }
    public InvoiceLineCollection InvoiceLines { get; } = new InvoiceLineCollection();
    public string Currency => "EUR";

    public bool IsFinalized { get; private set; }

    [MemberNotNull(nameof(InvoiceNumber), nameof(InvoiceDate))]
    public void Finalize(IUniqueInvoiceNumberSource invoiceNumberSource, ZonedClock clock)
    {
        InvoiceNumber = invoiceNumberSource.GetNextInvoiceNumber();
        InvoiceDate = clock.GetCurrentDate();
        SetFinalized();
    }

    public void SetFinalized() {
        IsFinalized = InvoiceNumber.HasValue;

        if (IsFinalized)
        {
            InvoiceLines.Freeze();
        }
    }

    public InvoiceTotals CalculateTotal() => new InvoiceTotals(InvoiceLines);
}
