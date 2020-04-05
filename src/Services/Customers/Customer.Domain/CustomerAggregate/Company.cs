namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public class Company : Customer
    {
        public override bool IsCompany => true;
        public override string Name => CompanyName;
        public string CompanyName { get; }
        public string VatNumber { get; }

        public Company(string companyName, string vatNumber, string countryCode)
            : base(countryCode)
        {
            CompanyName = companyName;
            VatNumber = vatNumber;
        }
    }
}
