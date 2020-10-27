namespace Invoicing.Customers.Api.EndPoints.GetCustomerById
{
    public class GetCusomerByIdResult
    {
        public GetCusomerByIdResult(string id, string name, string billingAddress, bool isCompany)
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