using InvoiceService.Data;
using InvoiceService.Data.Setup;
using Invoicing.Services.InvoiceService.Api.Infrastructure;
using Invoicing.Services.InvoiceService.Api.Invoices;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.ApplicationName = "Invoicing Service";

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Invoiceservice Api", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
});

// Add current tenant provider
builder.Services.AddScoped<RouteCurrentTenantProvider>();
builder.Services.AddScoped<ICurrentTenantProvider>(provider => provider.GetRequiredService<RouteCurrentTenantProvider>());
builder.Services.AddScoped<IHttpContextInitialized>(provider => provider.GetRequiredService<RouteCurrentTenantProvider>());

// Register NodaTime
builder.Services.AddSingleton<IClock>(SystemClock.Instance);
builder.Services.AddSingleton<ZonedClock>(provider => new ZonedClock(provider.GetRequiredService<IClock>(), DateTimeZoneProviders.Tzdb["Europe/Brussels"], CalendarSystem.Iso));
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

// Health checks
builder.Services.AddHealthChecks();

builder.AddTelemetry();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddInvoiceServiceDataAccess(builder.Configuration.GetConnectionString("Invoices"), builder.Configuration.GetConnectionString("StorageAccount"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add middleware for services that need initialization from a middleware
app.UseMiddleware<InitializeFromHttpContextMiddleware>();

app.MapGroup("/{tenantId:alpha}/invoices")
   .MapInvoiceApi()
   .WithTags("Invoices")
   .AddEndpointFilter<TracingActionFilter>();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes = 
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.Run();
