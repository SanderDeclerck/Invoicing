using Azure.Storage.Blobs;
using InvoiceService.Data.InvoiceIssuers.Entities;
using InvoiceService.Domain.InvoiceIssuers;
using InvoiceService.Domain.InvoiceIssuers.Interfaces;
using Microsoft.Azure.Cosmos;
using static InvoiceService.Data.Telemetry;

namespace InvoiceService.Data.InvoiceIssuers;

public class InvoiceIssuerRepository : IInvoiceIssuerRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly BlobServiceClient _blobServiceClient;

    private Container Container => _cosmosClient.GetContainer("InvoiceService", "InvoiceIssuers");
    private BlobContainerClient BlobContainer => _blobServiceClient.GetBlobContainerClient("invoice-issuers-logos");

    public InvoiceIssuerRepository(CosmosClient cosmosClient, ICurrentTenantProvider currentTenantProvider, BlobServiceClient blobServiceClient)
    {
        _cosmosClient = cosmosClient;
        _currentTenantProvider = currentTenantProvider;
        _blobServiceClient = blobServiceClient;
    }
   
    public async Task<InvoiceIssuer?> GetByName(string name, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("InvoiceIssuerRepository.GetByName", System.Diagnostics.ActivityKind.Internal);

        try
        {
            var tenant = _currentTenantProvider.GetTenantId();
            var query = new QueryDefinition("SELECT TOP 1 * FROM c WHERE c.tenantId = @tenantId AND c.name = @name")
                .WithParameter("@tenantId", tenant)
                .WithParameter("@name", name);

            var iterator = Container.GetItemQueryIterator<InvoiceIssuerEntity>(query, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(tenant) });
            
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                var entity = response.FirstOrDefault();

                if (entity != null)
                {
                    var address = new IssuerAddress(entity.StreetAndNumber, entity.PostalCode, entity.City, entity.Country);
                    var vatNumber = new VatNumber(entity.VatNumber, entity.VatRegistration);
                    var bankAccount = new BankAccount(entity.BankAccountIban, entity.BankAccountBic);

                    var logoBytes = new byte[0]; // TODO: Load logo from blob storage

                    return new InvoiceIssuer(entity.Id, entity.Name, address, vatNumber, bankAccount, entity.Email, entity.Phone, logoBytes);
                }
            }
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        return null;
    }


}
