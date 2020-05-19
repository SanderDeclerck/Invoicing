using System.Threading.Tasks;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Infrastructure.Data;
using Invoicing.Customers.Domain.CustomerAggregate;
using MongoDB.Driver;
using Base.Infrastructure;
using Microsoft.AspNetCore.Http;

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

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _customerMongoContext.GetCustomerCollection().InsertOneAsync(GetEntity(customer));
            return customer;
        }

        public async Task<Customer?> GetAsync(string customerId)
        {
            var customer = (await _customerMongoContext.GetCustomerCollection()
                                                       .FindAsync<TenantEntity<Customer>>(CreateCustomerByIdFilter(customerId)))
                                                       .FirstOrDefault();
            return customer?.Entity;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            await _customerMongoContext.GetCustomerCollection()
                                       .ReplaceOneAsync(CreateCustomerByIdFilter(customer.Id),
                                                        GetEntity(customer));
            return customer;
        }

        private FilterDefinition<TenantEntity<Customer>> CreateCustomerByIdFilter(string customerId)
        {
            return Builders<TenantEntity<Customer>>.Filter.And(
                        Builders<TenantEntity<Customer>>.Filter.Eq(c => c.Entity.Id, customerId),
                        Builders<TenantEntity<Customer>>.Filter.Eq(c => c.TenantId, TenantId));
        }
    }
}