using System.Threading.Tasks;
using Invoicing.Base.Ddd;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer Add(Customer customer);
        Customer Update(Customer customer);
        Task<Customer> GetAsync(int customerId);
    }
}
