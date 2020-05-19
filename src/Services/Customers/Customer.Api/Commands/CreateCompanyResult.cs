namespace Invoicing.Customers.Api.Commands
{
    public class CreateCompanyResult
    {
        public string Id { get; }
        public string Name { get; }
        public string VatNumber { get; }

        public CreateCompanyResult(string id, string name, string vatNumber)
        {
            Id = id;
            Name = name;
            VatNumber = vatNumber;
        }
    }
}