using System.Diagnostics;

namespace InvoiceService.Data;

public static class Telemetry 
{
    public const string ActivitySourceName = "Invoicing.Services.InvoiceService.Data";
    public static readonly ActivitySource ActivitySource = new ActivitySource(ActivitySourceName);
}