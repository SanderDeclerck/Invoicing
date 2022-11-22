using Microsoft.AspNetCore.Http.HttpResults;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class InvoiceApi {
    public const string CreateInvoiceRouteName = "Create invoice";
    public const string GetInvoiceByIdRouteName = "Get invoice by id";


    public static RouteGroupBuilder MapInvoiceApi(this RouteGroupBuilder group) {

        group.MapPost("/", CreateInvoice.Handle)
             .WithName(CreateInvoiceRouteName);

        group.MapGet("/{id:guid}", GetInvoiceById.Handle)
             .WithName(GetInvoiceByIdRouteName);

        return group;
    }
}
