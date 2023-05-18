namespace InvoiceService.Data.InvoiceNumberSources.Entities;

public class InvoiceNumberSourceEntity
{
    public required Guid Id { get; init; }
    public required string TenantId { get; init; }
    public required int CurrentNumber { get; init; }

    public required string _etag { get; init; }
}