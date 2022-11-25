using InvoiceService.Data.Invoices.Entities;
using InvoiceService.Data.Invoices.Mappers;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using Microsoft.Azure.Cosmos;

namespace InvoiceService.Data.Invoices;
public class InvoiceRepository : IInvoiceRepository
{
    private readonly CosmosClient _cosmosClient;

    private Container Container => _cosmosClient.GetContainer("InvoiceService", "Invoices");

    public InvoiceRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public async Task<Invoice?> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await Container.ReadItemAsync<InvoiceEntity>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
            return response.Resource.MapToDomain();
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task Insert(Invoice invoice, CancellationToken cancellationToken)
    {
        var entity = invoice.MapToEntity();
        await Container.CreateItemAsync(entity, new PartitionKey(entity.Id.ToString()), cancellationToken: cancellationToken);
    }

    public async Task Update(Invoice invoice, CancellationToken cancellationToken)
    {
        var entity = invoice.MapToEntity();
        await Container.UpsertItemAsync(entity, new PartitionKey(entity.Id.ToString()), cancellationToken: cancellationToken);
    }
}
