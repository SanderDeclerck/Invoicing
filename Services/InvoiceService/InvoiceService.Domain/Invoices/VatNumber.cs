using Invoicing.SharedKernel.Domain;

namespace Invoicing.Services.InvoiceService.Invoices.Domain;

public class VatNumber : ValueObject
{
    public string Number { get; }

    public VatNumber(string number)
    {
        Number = number;
    }
}