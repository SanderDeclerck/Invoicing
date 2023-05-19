using System.Diagnostics;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class GetInvoiceById 
{
    public class Log {}

    public record Response(
        Guid Id,
        ResponseCustomer Customer,
        LocalDate? InvoiceDate,
        int? InvoiceNumber,
        ResponseInvoiceTotals Totals,
        ResponseInvoiceLine[] InvoiceLines
    );

    public record ResponseCustomer(
        string Name,
        string StreetAndNumber,
        string PostalCode,
        string City,
        string Country,
        string? VatNumber
    );

    public record ResponseInvoiceLine(
        string Description,
        decimal Quantity,
        decimal UnitPrice,
        int VatPercentage,
        decimal TotalPrice
    );

    public record ResponseInvoiceTotals(
        decimal TotalIncludingVat,
        decimal TotalExcludingVat,
        decimal TotalVat
    );

    public static async Task<Results<Ok<Response>, NotFound>> Handle(
        [FromRoute] Guid id,
        [FromServices] IInvoiceRepository repository,
        [FromServices] ILogger<Log> logger,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        Activity.Current?.AddTag("app.invoiceId", id.ToString());
        
        var invoice = await repository.GetById(id, cancellationToken);

        if (invoice == null)
        {
            logger.LogWarning($"Invoice {id} not found");
            return TypedResults.NotFound();
        }

        var responseCustomer = new ResponseCustomer(
            invoice.Customer.Name,
            invoice.Customer.Address.StreetAndNumber,
            invoice.Customer.Address.PostalCode,
            invoice.Customer.Address.City,
            invoice.Customer.Address.Country,
            invoice.Customer.VatNumber?.Number);

        var responseInvoiceLines = invoice.InvoiceLines.Items.Select(invoiceLine => new ResponseInvoiceLine(
            invoiceLine.Description,
            invoiceLine.Quantity,
            invoiceLine.UnitPrice,
            invoiceLine.VatPercentage,
            invoiceLine.TotalIncludingVat)).ToArray();

        var invoiceTotals = invoice.CalculateTotal();
        var responseTotal = new ResponseInvoiceTotals(
            invoiceTotals.TotalIncludingVat,
            invoiceTotals.TotalExcludingVat,
            invoiceTotals.TotalVat
        );

        var response = new Response(
            invoice.Id,
            responseCustomer,
            invoice.InvoiceDate,
            invoice.InvoiceNumber,
            responseTotal,
            responseInvoiceLines);

        return TypedResults.Ok(response);
    }
}
