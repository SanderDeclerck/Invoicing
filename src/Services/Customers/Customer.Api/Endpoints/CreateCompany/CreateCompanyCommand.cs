namespace Invoicing.Customers.Api.Endpoints.CreateCompany
{
    public class CreateCompanyCommand
    {
        public string? CompanyName { get; set; }
        public string? VatNumber { get; set; }
    }
}