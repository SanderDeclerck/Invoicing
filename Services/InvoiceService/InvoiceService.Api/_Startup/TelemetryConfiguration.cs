using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class TelemetryConfiguration
{
    public const string ServiceName = "Invoicing";
    public static readonly string? Version = typeof(TelemetryConfiguration).Assembly.GetName().Version?.ToString();

    public static void AddTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddSource(ServiceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: ServiceName, serviceVersion: Version))
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Enrich = Enrich;
                })
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("https://api.honeycomb.io");
                    options.Headers = "x-honeycomb-team={{key}}";
                });
        });
    }

    public static void Enrich(Activity activity, string eventName, object rawObject)
    {
        HttpContext context;
        if (rawObject is HttpRequest request)
        {
            context = request.HttpContext;
        }
        else if (rawObject is HttpResponse response)
        {
            context = response.HttpContext;
        }
        else
        {
            return;
        }

        if (context.Features.Get<IEndpointFeature>()?.Endpoint is RouteEndpoint endpoint)
        {
            // We like display name like: GET /normalized-lower-case-route
            // so it's easier to differentiate and sort, but that's just our pref. You can
            // see how we get the route pattern, though.
            var routeName = $"{context.Request.Method} {endpoint.RoutePattern?.RawText}";
            activity.DisplayName = routeName;
            activity.SetTag("http.target", routeName);
        }
        else
        {
            // Fall back to path.
            var routeName = $"{context.Request.Method} {context.Request.Path.ToString().ToLowerInvariant()}";
            activity.DisplayName = routeName;
            activity.SetTag("http.target", routeName);
        }

    }
}
