namespace Invoicing.Services.InvoiceService.Api.Infrastructure;

interface IHttpContextInitialized 
{
    void Initialize(HttpContext httpContext);
}

class InitializeFromHttpContextMiddleware
{
    private readonly RequestDelegate _next;

    public InitializeFromHttpContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IEnumerable<IHttpContextInitialized> initializers)
    {
        foreach (var initializer in initializers)
        {
            initializer.Initialize(httpContext);
        }

        await _next(httpContext);
    }
}