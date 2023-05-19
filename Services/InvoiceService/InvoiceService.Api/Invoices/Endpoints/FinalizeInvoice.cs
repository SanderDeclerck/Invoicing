using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources.Invoices;
using System.Diagnostics;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class FinalizeInvoice
{
    public class Log {}

    public record Request(
        Guid InvoiceId
    );

    public record Response(
        Guid Id,
        int InvoiceNumber,
        LocalDate InvoiceDate
    );

    public static async Task<Results<Ok<Response>, NotFound>> Handle(
        [FromRoute] Guid id,
        [FromServices] IInvoiceRepository repository,
        [FromServices] IInvoiceNumberSourceRepository invoiceNumberSourceRepository,
        [FromServices] ZonedClock clock,
        [FromServices] ILogger<Log> logger,
        CancellationToken cancellationToken)
    {
        Activity.Current?.AddTag("app.invoiceId", id.ToString());
        
        var invoice = await repository.GetById(id, cancellationToken);

        if (invoice == null)
        {
            logger.LogWarning($"Invoice {id} not found");
            return TypedResults.NotFound();
        }

        var invoiceNumberSource = await invoiceNumberSourceRepository.GetOrCreate(cancellationToken);


        if (invoice.IsFinalized)
        {
            logger.LogWarning($"Invoice {invoice.Id} is already finalized");
        }
        else
        {
            invoice.Finalize(invoiceNumberSource, clock);

            await invoiceNumberSourceRepository.Update(invoiceNumberSource, cancellationToken);
            await repository.Update(invoice, cancellationToken);
        }

        return TypedResults.Ok(new Response(invoice.Id, invoice.InvoiceNumber!.Value, invoice.InvoiceDate!.Value));
    }
}