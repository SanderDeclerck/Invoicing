using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;

namespace Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources;

public class InvoiceNumberSource : IUniqueInvoiceNumberSource
{
    private string _concurrencyToken;

    public InvoiceNumberSource(Guid id, string concurrencyToken, int currentNumber)
    {
        Id = id;
        _concurrencyToken = concurrencyToken;
        CurrentNumber = currentNumber;
    }

    public Guid Id { get; private init; }
    public int CurrentNumber { get; private set; }

    public int GetNextInvoiceNumber()
    {
        return ++CurrentNumber;
    }

    public string GetConcurrencyToken()
    {
        return _concurrencyToken;
    }

    public void SetConcurrencyToken(string concurrencyToken)
    {
        _concurrencyToken = concurrencyToken;
    }
}
