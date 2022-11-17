using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

public static class TelemetryConfiguration
{
    public const string ServiceName = "Invoicing";
    public static readonly string? Version = typeof(TelemetryConfiguration).Assembly.GetName().Version?.ToString();

    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);

        builder.Logging.AddOpenTelemetry(logging => 
        {
            logging.SetResourceBuilder(resourceBuilder)
                   .AddConsoleExporter();
        });

        builder.Services.AddOpenTelemetryMetrics(metrics => 
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
                   .AddConsoleExporter();
        });

        builder.Services.AddOpenTelemetryTracing(traces => 
        {
            traces.SetResourceBuilder(resourceBuilder)
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddConsoleExporter();
        });
    }
}
