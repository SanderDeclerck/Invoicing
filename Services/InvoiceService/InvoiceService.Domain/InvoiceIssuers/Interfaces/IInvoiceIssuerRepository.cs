namespace InvoiceService.Domain.InvoiceIssuers.Interfaces;

public interface IInvoiceIssuerRepository
{
    Task<InvoiceIssuer> GetByName(string name, CancellationToken cancellationToken);
}
