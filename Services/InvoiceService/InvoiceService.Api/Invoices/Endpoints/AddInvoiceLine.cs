using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using FluentValidation;
using System.Diagnostics;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class AddInvoiceLine
{
    public class Log { }

    public record Request(
        Guid InvoiceId,
        string Description,
        decimal Quantity,
        decimal UnitPrice,
        int VatPercentage
    );

    public record Response(
        Guid Id
    );

    public static async Task<Results<CreatedAtRoute, NotFound, ValidationProblem>> Handle(
        [FromRoute] Guid id,
        [FromBody] Request request,
        [FromServices] IInvoiceRepository repository,
        [FromServices] ILogger<Log> logger,
        CancellationToken cancellationToken)
    {
        Activity.Current?.AddTag("app.invoiceId", id.ToString());

        var validationResult = new RequestValidator().Validate(request);

        if (!validationResult.IsValid)
        {
            logger.LogWarning($"Invalid request: {string.Join(", ", validationResult.Errors)}");
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var invoice = await repository.GetById(id, cancellationToken);

        if (invoice == null)
        {
            logger.LogWarning($"Invoice {id} not found");
            return TypedResults.NotFound();
        }

        if (invoice.IsFinalized)
        {
            logger.LogWarning($"It is not allowed to modify finalized invoices");
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { nameof(invoice.IsFinalized), new[] { "It is not allowed to modify finalized invoices" } }
            });
        }

        var invoiceLine = new InvoiceLine(
            description: request.Description,
            quantity: request.Quantity,
            unitPrice: request.UnitPrice,
            vatPercentage: request.VatPercentage);

        invoice.InvoiceLines.AddLine(invoiceLine);

        await repository.Update(invoice, cancellationToken);

        return TypedResults.CreatedAtRoute(InvoiceApi.GetInvoiceByIdRouteName, new { id = invoice.Id });
    }
}

file class RequestValidator : AbstractValidator<AddInvoiceLine.Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
        RuleFor(x => x.VatPercentage).GreaterThanOrEqualTo(0);
    }
}
