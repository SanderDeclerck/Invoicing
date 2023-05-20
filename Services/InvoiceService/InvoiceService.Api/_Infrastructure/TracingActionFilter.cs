using System.Diagnostics;
using InvoiceService.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Invoicing.Services.InvoiceService.Api.Infrastructure;

public class TracingActionFilter : IEndpointFilter
{
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public TracingActionFilter(ICurrentTenantProvider currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var tenantId = _currentTenantProvider.GetTenantId();
        Activity.Current?.AddTag("app.tenantId", tenantId.ToString());
        return next(context);
    }
}