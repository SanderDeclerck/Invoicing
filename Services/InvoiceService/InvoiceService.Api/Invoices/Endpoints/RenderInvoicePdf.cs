using System.Diagnostics;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using InvoiceService.Pdf;
using Microsoft.Extensions.Localization;
using System.Globalization;
using InvoiceService.Domain.InvoiceIssuers.Interfaces;
using InvoiceService.Data;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class RenderInvoicePdf 
{
    private static readonly CultureInfo DefaultLanguage = new("nl-BE");

    public static async Task<Results<FileStreamHttpResult, NotFound>> Handle(
        [FromRoute] Guid id,
        [FromServices] ICurrentTenantProvider currentTenantProvider,
        [FromServices] IInvoiceRepository invoiceRepository,
        [FromServices] IInvoiceIssuerRepository invoiceIssuerRepository,
        [FromServices] IStringLocalizer<InvoicePdfRenderer> localizer,
        CancellationToken cancellationToken)
    {
        Activity.Current?.SetTag("app.invoiceId", id);

        var tenant = currentTenantProvider.GetTenantId();
        var invoiceIssuer = await invoiceIssuerRepository.GetByName(tenant, cancellationToken);
        var invoice = await invoiceRepository.GetById(id, cancellationToken);

        if (invoiceIssuer == null)
        {
            return TypedResults.NotFound();
        }

        if (invoice == null)
        {
            return TypedResults.NotFound();
        }

        var pdfRenderer = new InvoicePdfRenderer(DefaultLanguage, localizer);
        var document = pdfRenderer.CreatePdf(invoice, invoiceIssuer);

        return TypedResults.File(document, "application/pdf", $"{invoice.InvoiceNumber}.pdf");
    }
}
