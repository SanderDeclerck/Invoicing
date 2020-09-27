namespace Invoicing.Customers.Api.CustomerEndpoints.GetById
{
    public class GetByIdResult
    {
        public GetByIdResult(string id, string name, string billingAddress, bool isCompany)
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