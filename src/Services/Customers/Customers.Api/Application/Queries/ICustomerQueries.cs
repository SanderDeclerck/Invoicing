using System.Collections.Generic;

namespace Customers.Api.Application.Queries
{
    public interface ICustomerQueries
    {
        CustomerViewModel GetCustomerById(int id);
        List<CustomerViewModelSlim> GetAllCustomers();
    }
}
