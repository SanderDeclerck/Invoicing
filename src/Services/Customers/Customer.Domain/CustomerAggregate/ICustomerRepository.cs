using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Invoicing.Base.Ddd;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer?> GetAsync(string customerId, CancellationToken cancellationToken);
        Task<List<Customer>> GetList(CancellationToken cancellationToken);
    }
}