using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Infrastructure.Data;
using Invoicing.Customers.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Invoicing.Customers.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomersInfrastructure(this IServiceCollection services, string connectionString)
        {
            var mongoConfig = new CustomerCollectionConfigurator();
            mongoConfig.Setup();

            services.AddScoped<ICustomerMongoContext>(_ => new CustomerMongoContext(connectionString));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}