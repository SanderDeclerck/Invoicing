using FluentValidation;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class CreateInvoice
{
    public record Request(
        string CustomerName,
        string StreetAndNumber,
        string PostalCode,
        string City,
        string? Country,
        string? VatNumber
    );

    public record Response(
        Guid Id
    );

    public static async Task<Results<CreatedAtRoute, ValidationProblem>> Handle(
        [FromBody] Request request,
        [FromServices] IInvoiceRepository repository,
        CancellationToken cancellationToken)
    {
        var validationResult = new RequestValidator().Validate(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var customer = CreateCustomer(request);
        var invoice = Invoice.CreateInvoiceForCustomer(customer);

        await repository.Insert(invoice, cancellationToken);

        return TypedResults.CreatedAtRoute(InvoiceApi.GetInvoiceByIdRouteName, new { id = invoice.Id });
    }

    private static Customer CreateCustomer(Request request)
    {
        const string DefaultCountry = "Belgium";

        var address = new InvoiceAddress(
            streetAndNumber: request.StreetAndNumber,
            postalCode: request.PostalCode,
            city: request.City,
            country: request.Country ?? DefaultCountry);

        if (request.VatNumber == null)
        {
            return new Customer(
                name: request.CustomerName,
                address: address
            );
        }
        else
        {
            return new Customer(
                name: request.CustomerName,
                address: address,
                vatNumber: new VatNumber(request.VatNumber)
            );
        }
    }
}

file class RequestValidator : AbstractValidator<CreateInvoice.Request>
{
    public RequestValidator()
    {
        RuleFor(req => req.CustomerName).NotEmpty();
        RuleFor(req => req.StreetAndNumber).NotEmpty();
        RuleFor(req => req.PostalCode).NotEmpty();
        RuleFor(req => req.City).NotEmpty();
    }
}
