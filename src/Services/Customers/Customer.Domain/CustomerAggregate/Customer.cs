using Invoicing.Base.Ddd;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CompanyName { get; private set; }
        public string VatNumber { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
        public Address BillingAddress { get; private set; }
        public Address ShippingAddress { get; private set; }

        private Customer() : this("", "", "", "", "", "")
        {
        }

        public Customer(string firstName, string lastName, string companyName, string vatNumber, string phoneNumber, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            VatNumber = vatNumber;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;

            BillingAddress = Address.Empty;
            ShippingAddress = Address.Empty;
        }

        public void SetBillingAddress(Address address)
        {
            BillingAddress = address;
        }

        public void SetShippingAddress(Address address)
        {
            ShippingAddress = address;
        }
    }
}
