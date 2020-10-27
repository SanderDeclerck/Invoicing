using System.Collections.Generic;
using System.Collections.Immutable;


namespace Invoicing.Customers.Api.Endpoints.GetCustomerList
{
    public class GetCustomerListResult
    {
        public GetCustomerListResult(IEnumerable<Customer> customers)
        {
            Customers = customers.ToImmutableList();
        }

        public ImmutableList<Customer> Customers { get; }

        public class Customer
        {
            public Customer(string id, string name, string billingAddress, bool isCompany)
            {
                Id = id;
                Name = name;
                BillingAddress = billingAddress;
                IsCompany = isCompany;
            }
            public string Id { get; }
            public string Name { get; }
            public string BillingAddress { get; }
            public bool IsCompany { get; }
        }
    }
}