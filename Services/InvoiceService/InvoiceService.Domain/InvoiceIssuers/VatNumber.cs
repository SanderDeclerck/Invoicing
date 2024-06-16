using Invoicing.SharedKernel.Domain;

namespace InvoiceService.Domain.InvoiceIssuers;

public class VatNumber : ValueObject
{
    public string Number { get; }
    public string Registration { get; }

    public VatNumber(string number, string registration)
    {
        Number = number;
        Registration = registration;
    }
}
