using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class Customer : ValueObject
{
    public Customer(string name, InvoiceAddress address)
    {
        Name = name;
        Address = address;
    }

    public Customer(string name, InvoiceAddress address, VatNumber vatNumber)
    {
        Name = name;
        Address = address;
        VatNumber = vatNumber;
    }

    public string Name { get; }
    public InvoiceAddress Address { get; }
    public VatNumber? VatNumber { get; }
}
