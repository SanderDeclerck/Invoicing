namespace Invoicing.Customers.Domain.CustomerAggregate
{
    public class PrivateIndividual : Customer
    {
        public override bool IsCompany => false;
        public override string Name => $"{FirstName} {LastName}";
        public string FirstName { get; }
        public string LastName { get; }

        public PrivateIndividual(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
