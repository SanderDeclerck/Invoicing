using InvoiceService.Data.Invoices.Entities;
using InvoiceService.Data.Invoices.Mappers;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using Microsoft.Azure.Cosmos;
using static InvoiceService.Data.Telemetry;

namespace InvoiceService.Data.Invoices;
public class InvoiceRepository : IInvoiceRepository
{
    private readonly CosmosClient _cosmosClient;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    private Container Container => _cosmosClient.GetContainer("InvoiceService", "Invoices");

    public InvoiceRepository(CosmosClient cosmosClient, ICurrentTenantProvider currentTenantProvider)
    {
        _cosmosClient = cosmosClient;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task<Invoice?> GetById(Guid id, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("InvoiceRepository.GetById", System.Diagnostics.ActivityKind.Internal);
        try
        {
            var tenant = _currentTenantProvider.GetTenantId();
            var response = await Container.ReadItemAsync<InvoiceEntity>(id.ToString(), new PartitionKey(tenant), cancellationToken: cancellationToken);
            return response.Resource.MapToDomain();
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task Insert(Invoice invoice, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("InvoiceRepository.Insert", System.Diagnostics.ActivityKind.Internal);
        var tenantId = _currentTenantProvider.GetTenantId();
        var entity = invoice.MapToEntity(tenantId);
        await Container.CreateItemAsync(entity, new PartitionKey(tenantId), cancellationToken: cancellationToken);
    }

    public async Task Update(Invoice invoice, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("InvoiceRepository.Update", System.Diagnostics.ActivityKind.Internal);
        var tenantId = _currentTenantProvider.GetTenantId();
        var entity = invoice.MapToEntity(tenantId);
        await Container.UpsertItemAsync(entity, new PartitionKey(tenantId), cancellationToken: cancellationToken);
    }
}
