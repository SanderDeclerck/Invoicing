using Customers.Api.Application.Queries;

namespace Customers.Api.Application.Commands.CreateCustomer
{
    public class CommandResult
    {
        public bool IsSuccess { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
