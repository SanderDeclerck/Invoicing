using System.Threading.Tasks;
using Invoicing.Base.Ddd;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<Customer?> GetAsync(string customerId);
    }
}