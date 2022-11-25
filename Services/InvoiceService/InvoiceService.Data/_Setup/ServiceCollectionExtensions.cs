using InvoiceService.Data.Invoices;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Data.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvoiceServiceDataAccess(this IServiceCollection services, string? invoiceCosmosConnectionString)
    {
        if (string.IsNullOrEmpty(invoiceCosmosConnectionString))
        {
            throw new ArgumentException("Cosmos connection string is required", nameof(invoiceCosmosConnectionString));
        }

        services.AddSingleton(new CosmosClient(invoiceCosmosConnectionString, new CosmosClientOptions
        {
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        }));
        
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        
        return services;
    }
}