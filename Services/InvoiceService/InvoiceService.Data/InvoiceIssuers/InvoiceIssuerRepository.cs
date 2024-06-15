using InvoiceService.Domain.InvoiceIssuers;
using InvoiceService.Domain.InvoiceIssuers.Interfaces;

namespace InvoiceService.Data.InvoiceIssuers
{
    public class InvoiceIssuerRepository : IInvoiceIssuerRepository
    {
        public Task<InvoiceIssuer> GetByName(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}