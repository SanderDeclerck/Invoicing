using System.Threading;
using System.Threading.Tasks;
using Base.Infrastructure;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Domain.CustomerAggregate;
using MongoDB.Driver;

namespace Invoicing.Customers.Infrastructure.Data
{
    public interface ICustomerMongoContext : IUnitOfWork
    {
        IMongoCollection<TenantEntity<Customer>> GetCustomerCollection();
    }

    public class CustomerMongoContext : ICustomerMongoContext
    {
        private const string CustomerDatabaseName = "Customers";
        private const string CustomerCollectionName = "Customers";

        private readonly IMongoClient _mongoClient;
        private IClientSession? _mongoSession;

        public CustomerMongoContext(string mongoConnectionString)
        {
            _mongoClient = new MongoClient(mongoConnectionString);
        }

        public IMongoCollection<TenantEntity<Customer>> GetCustomerCollection()
        {
            var database = _mongoClient.GetDatabase(CustomerDatabaseName);
            var customers = database.GetCollection<TenantEntity<Customer>>(CustomerCollectionName);

            return customers;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var session = await GetSession();
            await session.CommitTransactionAsync();
        }

        public void Dispose()
        {
            var session = GetSession().Result;
            session.Dispose();
        }

        private async Task<IClientSession> GetSession()
        {
            if (_mongoSession == null)
            {
                _mongoSession = await _mongoClient.StartSessionAsync();
            }
            if (!_mongoSession.IsInTransaction)
            {
                _mongoSession.StartTransaction();
            }
            return _mongoSession;
        }
    }
}