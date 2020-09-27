namespace Invoicing.Customers.Api.CustomerEndpoints.CreateCompany
{
    public class CreateCompanyCommand
    {
        public string? CompanyName { get; set; }
        public string? VatNumber { get; set; }
    }
}