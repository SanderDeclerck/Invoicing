using Microsoft.AspNetCore.Http.HttpResults;

namespace Invoicing.Services.InvoiceService.Api.Invoices;

public static class GetInvoiceById {
    public record Response(
        Guid Id
    );

    public static Results<Ok<Response>, NotFound> Handle(Guid id) 
    {
        return TypedResults.Ok(new Response(id));
    }
}
