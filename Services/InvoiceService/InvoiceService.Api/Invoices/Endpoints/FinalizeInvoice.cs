using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using NodaTime;
using Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources.Invoices;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class FinalizeInvoice
{
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
        CancellationToken cancellationToken)
    {
        var invoice = await repository.GetById(id, cancellationToken);

        if (invoice == null)
        {
            return TypedResults.NotFound();
        }

        var invoiceNumberSource = await invoiceNumberSourceRepository.GetOrCreate(cancellationToken);


        if (!invoice.IsFinalized) {
            invoice.Finalize(invoiceNumberSource, clock);

            await invoiceNumberSourceRepository.Update(invoiceNumberSource, cancellationToken);
            await repository.Update(invoice, cancellationToken);
        }

        return TypedResults.Ok(new Response(invoice.Id, invoice.InvoiceNumber!.Value, invoice.InvoiceDate!.Value));
    }
}