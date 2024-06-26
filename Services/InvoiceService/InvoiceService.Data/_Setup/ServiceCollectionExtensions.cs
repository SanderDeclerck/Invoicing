using Azure.Storage.Blobs;
using InvoiceService.Data.InvoiceIssuers;
using InvoiceService.Data.InvoiceNumberSources;
using InvoiceService.Data.Invoices;
using InvoiceService.Domain.InvoiceIssuers.Interfaces;
using Invoicing.Services.InvoiceService.Domain.InvoiceNumberSources.Invoices;
using Invoicing.Services.InvoiceService.Domain.Invoices.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceService.Data.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInvoiceServiceDataAccess(this IServiceCollection services, string? invoiceCosmosConnectionString, string? blobStorageConnectionString)
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

        if (string.IsNullOrEmpty(blobStorageConnectionString))
        {
            throw new ArgumentException("Blob storage connection string is required", nameof(blobStorageConnectionString));
        }

        services.AddScoped(provider => new BlobServiceClient(blobStorageConnectionString));
        
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceNumberSourceRepository, InvoiceNumberSourceRepository>();
        services.AddScoped<IInvoiceIssuerRepository, InvoiceIssuerRepository>();
        
        return services;
    }
}