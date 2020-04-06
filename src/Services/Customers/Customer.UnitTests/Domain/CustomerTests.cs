using Bogus;
using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Domain.CustomerAggregate.Exceptions;
using Xunit;

namespace Invoicing.Customers.UnitTests.Domain
{
    public class CustomerTests
    {
        private readonly Faker _faker = new Faker("nl_BE");

        [Fact]
        public void Company_WhenCreated_SetsInitialFields()
        {
            var companyName = _faker.Company.CompanyName();
            var vatId = "BE0123.456.789";

            var customer = new Company(companyName, vatId);

            Assert.Equal(companyName, customer.CompanyName);
            Assert.Equal(companyName, customer.Name);
            Assert.Equal(vatId, customer.VatNumber);
            Assert.True(customer.IsCompany);

            Assert.Null(customer.BillingAddress);
            Assert.Null(customer.PhoneNumber);
            Assert.Null(customer.EmailAddress);
        }

        [Fact]
        public void PrivateIndividual_WhenCreated_SetsInitialFields()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;

            var customer = new PrivateIndividual(firstName, lastName);

            Assert.Equal(firstName, customer.FirstName);
            Assert.Equal(lastName, customer.LastName);
            Assert.Equal($"{firstName} {lastName}", customer.Name);
            Assert.False(customer.IsCompany);

            Assert.Null(customer.BillingAddress);
            Assert.Null(customer.PhoneNumber);
            Assert.Null(customer.EmailAddress);
        }

        [Fact]
        public void Company_SetBillingAddress_UpdatesBillingAddress()
        {
            var companyName = _faker.Company.CompanyName();
            var vatId = "BE0123.456.789";
            var customer = new Company(companyName, vatId);

            var address = new Address(_faker.Address.StreetAddress(), _faker.Address.City(), _faker.Address.ZipCode(), "BEL");
            customer.SetBillingAddress(address);

            Assert.Equal(address, customer.BillingAddress);
        }

        [Fact]
        public void PrivateIndividual_SetBillingAddress_UpdatesBillingAddress()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var customer = new PrivateIndividual(firstName, lastName);

            var address = new Address(_faker.Address.StreetAddress(), _faker.Address.City(), _faker.Address.ZipCode(), "BEL");
            customer.SetBillingAddress(address);

            Assert.Equal(address, customer.BillingAddress);
        }

        [Fact]
        public void Company_SetPhoneNumber_UpdatesPhoneNumber()
        {
            var companyName = _faker.Company.CompanyName();
            var vatId = "BE0123.456.789";
            var customer = new Company(companyName, vatId);
            var number = _faker.Phone.PhoneNumber();

            customer.SetPhoneNumber(number);

            Assert.Equal(number, customer.PhoneNumber);
        }

        [Fact]
        public void PrivateIndividual_SetPhoneNumber_UpdatesPhoneNumber()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var customer = new PrivateIndividual(firstName, lastName);
            var number = _faker.Phone.PhoneNumber();

            customer.SetPhoneNumber(number);

            Assert.Equal(number, customer.PhoneNumber);
        }

        [Fact]
        public void Company_SetEmailAddress_UpdatesEmailAddress()
        {
            var companyName = _faker.Company.CompanyName();
            var vatId = "BE0123.456.789";
            var customer = new Company(companyName, vatId);
            var email = _faker.Person.Email;

            customer.SetEmailAddress(email);

            Assert.Equal(email, customer.EmailAddress);
        }

        [Fact]
        public void PrivateIndividual_SetEmailAddress_UpdatesEmailAddress()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var customer = new PrivateIndividual(firstName, lastName);
            var email = _faker.Person.Email;

            customer.SetEmailAddress(email);

            Assert.Equal(email, customer.EmailAddress);
        }

        [Fact]
        public void Company_SetIncorrectEmailAddress_Throws()
        {
            var companyName = _faker.Company.CompanyName();
            var vatId = "BE0123.456.789";
            var customer = new Company(companyName, vatId);

            var ex = Assert.ThrowsAny<CustomerInvalidEmailException>(() => customer.SetEmailAddress("blah.be"));
        }

        [Fact]
        public void PrivateIndividual_SetIncorrectEmailAddress_Throws()
        {
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var customer = new PrivateIndividual(firstName, lastName);
            var email = _faker.Person.Email;

            var ex = Assert.ThrowsAny<CustomerInvalidEmailException>(() => customer.SetEmailAddress("blah.be"));
        }
    }
}