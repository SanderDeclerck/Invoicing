using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Services.InvoiceService.Api.Endpoints.Invoices;

public static class CreateInvoice
{
    private static readonly CreateInvoiceRequestValidator _validator = new CreateInvoiceRequestValidator();

    public static async Task<IResult> Handle(
        [FromBody] CreateInvoiceRequest request,
        [FromServices] IInvoiceRepository repository,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var customer = CreateCustomer(request);
        var invoice = Invoice.CreateInvoiceForCustomer(customer);

        await repository.Insert(invoice, cancellationToken);

        return Results.CreatedAtRoute(EndpointConfiguration.GetInvoiceRoute, new { id = invoice.Id });
    }

    private static Customer CreateCustomer(CreateInvoiceRequest request)
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

public record CreateInvoiceRequest(
    [Required]
    string CustomerName,
    string StreetAndNumber,
    string PostalCode,
    string City,
    string? Country,
    string? VatNumber
);

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(req => req.CustomerName).NotEmpty();
        RuleFor(req => req.StreetAndNumber).NotEmpty();
        RuleFor(req => req.PostalCode).NotEmpty();
        RuleFor(req => req.City).NotEmpty();
    }
}