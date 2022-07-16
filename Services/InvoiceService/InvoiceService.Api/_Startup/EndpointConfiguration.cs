
using Invoicing.Services.InvoiceService.Api.Endpoints.Invoices;

public static class EndpointConfiguration
{
    public const string InvoicingOpenApiTag = "Invoicing";

    public const string CreateInvoiceRoute = "create-invoice";
    public const string GetInvoiceRoute = "get-invoice";

    public static void MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("/invoices", CreateInvoice.Handle)
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest, typeof(HttpValidationProblemDetails), "application/problem+json")
            .WithName(CreateInvoiceRoute)
            .WithTags(InvoicingOpenApiTag);

        routeBuilder
            .MapGet("/invoices/{id:guid}", (Guid id) => Results.Ok())
            .Produces(StatusCodes.Status200OK, null, "application/json")
            .Produces(StatusCodes.Status404NotFound)
            .WithName(GetInvoiceRoute)
            .WithTags(InvoicingOpenApiTag);
    }
}