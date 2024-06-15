using Invoicing.SharedKernel.Domain;

namespace InvoiceService.Domain.InvoiceIssuers;

public class BankAccount : ValueObject
{
    public string Iban { get; }
    public string Bic { get; }

    public BankAccount(string iban, string bic)
    {
        Iban = iban;
        Bic = bic;
    }
}