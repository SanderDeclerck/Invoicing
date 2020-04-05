using System;
using System.Net.Mail;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Domain.CustomerAggregate.Exceptions;

namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public abstract class Customer : Entity, IAggregateRoot
    {
        public string? PhoneNumber { get; private set; }
        public string? EmailAddress { get; private set; }
        public string CountryCode { get; private set; }
        public Address? BillingAddress { get; private set; }
        public abstract bool IsCompany { get; }
        public abstract string Name { get; }

        public Customer(string countryCode)
        {
            CountryCode = countryCode;
        }

        public virtual void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public virtual void SetEmailAddress(string emailAddress)
        {
            if (!CheckEmailAddress(emailAddress))
            {
                throw new CustomerInvalidEmailException();
            }

            EmailAddress = emailAddress;
        }

        public virtual void SetBillingAddress(Address address)
        {
            BillingAddress = address;
        }

        private bool CheckEmailAddress(string emailAddress)
        {
            try
            {
                new MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
