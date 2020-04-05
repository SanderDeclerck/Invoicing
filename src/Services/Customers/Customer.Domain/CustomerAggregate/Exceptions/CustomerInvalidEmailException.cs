
using Invoicing.Base.Ddd;

namespace Invoicing.Customers.Domain.CustomerAggregate.Exceptions
{
    public class CustomerInvalidEmailException : DomainException
    {
        public CustomerInvalidEmailException()
        {
        }

        public override string Message => "It is not possible to set an inproper email address on a customer.";
    }
}