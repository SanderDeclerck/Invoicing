namespace InvoiceService.Data.InvoiceIssuers.Entities;

public class InvoiceIssuerEntity
{
    public required Guid Id { get; init; }
    public required string TenantId { get; init; }
    public required string Name { get; init; }
    public required string StreetAndNumber { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    public required string Country { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required string VatNumber { get; init; }
    public required string VatRegistration { get; init; }
    public required string BankAccountIban { get; init; }
    public required string BankAccountBic { get; init; }
    public required string LogoBlobName { get; init; }
}
