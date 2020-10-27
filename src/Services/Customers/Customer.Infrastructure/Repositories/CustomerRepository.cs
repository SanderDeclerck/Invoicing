using System.Threading.Tasks;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Infrastructure.Data;
using Invoicing.Customers.Domain.CustomerAggregate;
using MongoDB.Driver;
using Base.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Invoicing.Customers.Infrastructure.Repositories
{
    public class CustomerRepository : TenantRepository<Customer>, ICustomerRepository
    {
        private readonly ICustomerMongoContext _customerMongoContext;

        public CustomerRepository(ICustomerMongoContext customerMongoContext, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _customerMongoContext = customerMongoContext;
        }

        public override IUnitOfWork UnitOfWork => _customerMongoContext;

        public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _customerMongoContext.GetCustomerCollection().InsertOneAsync(WrapForTenant(customer), cancellationToken: cancellationToken);
            return customer;
        }

        public async Task<Customer?> GetAsync(string customerId, CancellationToken cancellationToken)
        {
            var customer = (await _customerMongoContext.GetCustomerCollection()
                                                       .FindAsync<TenantEntity<Customer>>(CreateCustomerByIdFilter(customerId), cancellationToken: cancellationToken))
                                                       .FirstOrDefault();
            return customer?.Entity;
        }


        public async Task<List<Customer>> GetList(CancellationToken cancellationToken)
        {
            var customers = await _customerMongoContext.GetCustomerCollection()
                                                       .FindAsync<TenantEntity<Customer>>(CreateTenantFilter(), cancellationToken: cancellationToken);
            return customers.ToEnumerable().Select(record => record.Entity).ToList();
        }

        public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _customerMongoContext.GetCustomerCollection()
                                       .ReplaceOneAsync(CreateCustomerByIdFilter(customer.Id),
                                                        WrapForTenant(customer),
                                                        cancellationToken: cancellationToken);
            return customer;
        }

        private FilterDefinition<TenantEntity<Customer>> CreateCustomerByIdFilter(string customerId)
        {
            return Builders<TenantEntity<Customer>>.Filter.And(
                        Builders<TenantEntity<Customer>>.Filter.Eq(c => c.Entity.Id, customerId),
                        CreateTenantFilter());
        }

        private FilterDefinition<TenantEntity<Customer>> CreateTenantFilter()
        {
            return Builders<TenantEntity<Customer>>.Filter.Eq(c => c.TenantId, TenantId);
        }
    }
}