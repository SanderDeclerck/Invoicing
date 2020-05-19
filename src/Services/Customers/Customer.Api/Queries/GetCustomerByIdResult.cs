using MediatR;

namespace Invoicing.Customers.Api.Queries
{
    public class GetCustomerByIdResult : IRequest
    {
        public bool IsFound { get; private set; }
        public string Id { get; }
        public string Name { get; }
        public string BillingAddress { get; }
        public bool IsCompany { get; }

        public GetCustomerByIdResult(string id, string name, string billingAddress, bool isCompany)
        {
            Id = id;
            Name = name;
            BillingAddress = billingAddress;
            IsCompany = isCompany;
            IsFound = true;
        }

        public static GetCustomerByIdResult NotFound(string id) => new GetCustomerByIdResult(id, string.Empty, string.Empty, false) { IsFound = false };
    }
}