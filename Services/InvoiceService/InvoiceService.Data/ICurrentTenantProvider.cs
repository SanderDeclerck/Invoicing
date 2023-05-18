namespace InvoiceService.Data;

public interface ICurrentTenantProvider
{
    string GetTenantId();
}