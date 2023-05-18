using InvoiceService.Data;

namespace Invoicing.Services.InvoiceService.Api.Infrastructure;

class RouteCurrentTenantProvider : ICurrentTenantProvider, IHttpContextInitialized
{
    private string? _tenantId;

    public void Initialize(HttpContext httpContext)
    {
        _tenantId = httpContext.Request.RouteValues["tenantId"]?.ToString();
    }

    public string GetTenantId()
    {
        if (_tenantId == null)
        {
            throw new TenantNotSpecifiedException();
        }

        return _tenantId;
    }
}

class TenantNotSpecifiedException : Exception
{
    public TenantNotSpecifiedException() : base("Tenant not specified in the route")
    {
    }
}
