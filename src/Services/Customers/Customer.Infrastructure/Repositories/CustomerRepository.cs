using Invoicing.Base.Ddd;
using System.Threading.Tasks;
using Invoicing.Customers.Domain.CustomerAggregate;

namespace Invoicing.Customers.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _customerDbContext;
        public IUnitOfWork UnitOfWork => _customerDbContext;

        public CustomerRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public Customer Add(Customer customer)
        {
            if (customer.IsTransient())
            {
                customer = _customerDbContext.Customers.Add(customer).Entity;
            }

            return customer;
        }

        public Customer Update(Customer customer)
        {
            return _customerDbContext.Customers.Update(customer).Entity;
        }

        public Task<Customer> GetAsync(int customerId)
        {
            var customer = _customerDbContext.Customers.Find(customerId);

            return Task.FromResult(customer);
        }
    }
}
