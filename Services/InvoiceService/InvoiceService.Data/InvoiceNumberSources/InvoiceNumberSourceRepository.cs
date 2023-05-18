
using InvoiceService.Data.InvoiceNumberSources.Entities;
using Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources;
using Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources.Invoices;
using Microsoft.Azure.Cosmos;

namespace InvoiceService.Data.InvoiceNumberSources;

public class InvoiceNumberSourceRepository : IInvoiceNumberSourceRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    private Container Container => _cosmosClient.GetContainer("InvoiceService", "InvoiceNumberSources");

    public InvoiceNumberSourceRepository(CosmosClient cosmosClient, ICurrentTenantProvider currentTenantProvider)
    {
        _cosmosClient = cosmosClient;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task<InvoiceNumberSource> GetOrCreate(CancellationToken cancellationToken)
    {
        var tenantId = _currentTenantProvider.GetTenantId();
        
        var query = new QueryDefinition("SELECT TOP 1 * FROM c WHERE c.tenantId = @tenantId ORDER BY c._ts DESC")
            .WithParameter("@tenantId", tenantId);
        
        var iterator = Container.GetItemQueryIterator<InvoiceNumberSourceEntity>(query, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(tenantId) });
        var invoiceNumberSourceEntity = (await iterator.ReadNextAsync(cancellationToken)).FirstOrDefault();

        if (invoiceNumberSourceEntity == null)
        {
            var entity = new InvoiceNumberSourceEntity
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CurrentNumber = 0,
                _etag = Guid.NewGuid().ToString()
            };
            var respose = await Container.CreateItemAsync(entity, new PartitionKey(tenantId), cancellationToken: cancellationToken);
            return new InvoiceNumberSource(entity.Id, respose.ETag, entity.CurrentNumber);
        }
        
        return new InvoiceNumberSource(invoiceNumberSourceEntity.Id, invoiceNumberSourceEntity._etag, invoiceNumberSourceEntity.CurrentNumber);
    }

    public async Task Update(InvoiceNumberSource invoiceNumberSource, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenantProvider.GetTenantId();
        var entity = new InvoiceNumberSourceEntity
        {
            Id = invoiceNumberSource.Id,
            TenantId = tenantId,
            CurrentNumber = invoiceNumberSource.CurrentNumber,
            _etag = invoiceNumberSource.GetConcurrencyToken()
        };
        var response = await Container.ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(tenantId), new ItemRequestOptions { IfMatchEtag = invoiceNumberSource.GetConcurrencyToken() }, cancellationToken);
        invoiceNumberSource.SetConcurrencyToken(response.ETag);
    }
}