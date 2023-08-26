using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using System.Diagnostics;

namespace AuthenticationService.Web.Startup;

public static class Telemetry
{
    public static readonly string? Version = typeof(Telemetry).Assembly.GetName().Version?.ToString();

    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var honeycombConfiguration = GetHoneycombConfiguration(builder.Configuration);

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName, serviceVersion: Version);

        builder.Logging.AddOpenTelemetry(logging => 
        {
            logging.SetResourceBuilder(resourceBuilder)
                   ;

            if (honeycombConfiguration != null) 
            {
                logging.AddOtlpExporter(options => 
                {
                    options.Endpoint = honeycombConfiguration.Value.Endpoint;
                    options.Headers = honeycombConfiguration.Value.Headers;
                });
            }
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(traces => 
            {
                traces.SetResourceBuilder(resourceBuilder)
                      .AddAspNetCoreInstrumentation(options => 
                      {
                        options.EnrichWithHttpResponse = (activity, response) => 
                        {
                            SetActivityRoute(activity, response);
                        };
                      })
                      .AddHttpClientInstrumentation()
                      ;

                if (honeycombConfiguration != null) 
                {
                    traces.AddOtlpExporter(options => 
                    {
                        options.Endpoint = honeycombConfiguration.Value.Endpoint;
                        options.Headers = honeycombConfiguration.Value.Headers;
                    });
                }
            })
            .WithMetrics(metrics => 
            {
                metrics.SetResourceBuilder(resourceBuilder)
                       .AddAspNetCoreInstrumentation()
                       .AddRuntimeInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddEventCountersInstrumentation(c =>
                       {
                           // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                           c.AddEventSources(
                               "Microsoft.AspNetCore.Hosting",
                               // There's currently a bug preventing this from working
                               // "Microsoft-AspNetCore-Server-Kestrel"
                               "System.Net.Http", 
                               "System.Net.Sockets",
                               "System.Net.NameResolution",
                               "System.Net.Security");
                       })
                       ;

                if (honeycombConfiguration != null) 
                {
                    metrics.AddOtlpExporter(options => 
                    {
                        options.Endpoint = honeycombConfiguration.Value.Endpoint;
                        options.Headers = honeycombConfiguration.Value.Headers;
                    });
                }
            });
    }

    private static (Uri Endpoint, string Headers)? GetHoneycombConfiguration(IConfiguration configuration) 
    {
        var honeycombTelemetryConfiguration = configuration.GetSection("Telemetry").GetSection("Honeycomb").Get<HoneycombConfiguration>();

        if (honeycombTelemetryConfiguration == null) 
        {
            return null;
        }
        
        return (honeycombTelemetryConfiguration.Uri, $"x-honeycomb-team={honeycombTelemetryConfiguration.ApiKey}");
    }

    private static void SetActivityRoute(Activity activity, HttpResponse response) 
    {
        if (response.HttpContext.GetEndpoint() is RouteEndpoint routeEndpoint) 
        {
            activity.SetTag("http.route", $"{response.HttpContext.Request.Method} {routeEndpoint.RoutePattern.RawText}");
        }
        else 
        {
            activity.SetTag("http.route", $"{response.HttpContext.Request.Method} {response.HttpContext.Request.Path}");
        }
    }
}


file record HoneycombConfiguration(Uri Uri, string ApiKey);
