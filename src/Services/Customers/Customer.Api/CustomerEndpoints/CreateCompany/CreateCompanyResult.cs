namespace Invoicing.Customers.Api.CustomerEndpoints.CreateCompany
{
    public class CreateCompanyResult
    {
        public CreateCompanyResult(string id, string name, string vatNumber)
        {
            Id = id;
            Name = name;
            VatNumber = vatNumber;
        }

        public string Id { get; }
        public string Name { get; }
        public string VatNumber { get; }
    }
}