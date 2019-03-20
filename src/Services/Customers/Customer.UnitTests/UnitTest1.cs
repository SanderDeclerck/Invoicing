using System;
using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Infrastructure;
using Invoicing.Customers.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Customers.UnitTests
{
    public class UnitTest1
    {
        private ICustomerRepository _customerRepository;

        [Fact]
        public async void Test1()
        {
            Init();

            //var customer = new Customer("Sander", "Declerck", "SD Software", "BE0716.943.826", "+32472424602",
            //    "sander@sdsoftware.be");

            //_customerRepository.Add(customer);

            await _customerRepository.UnitOfWork.SaveEntitiesAsync();
            
            var foundCustomer = await _customerRepository.GetAsync(3);

            foundCustomer.SetShippingAddress(new Address("Galgestraat 33a", "Langemark-Poelkapelle", "8920", "BEL"));

            await _customerRepository.UnitOfWork.SaveEntitiesAsync();

            foundCustomer.SetBillingAddress(new Address("Galgestraat 33a", "Langemark-Poelkapelle", "8920", "BEL"));

            await _customerRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public void Init()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
            optionsBuilder.UseSqlite("Data Source=E:\\invoicing.db");
            
            var dbContext = new CustomerDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            _customerRepository = new CustomerRepository(dbContext);
        }
    }
}
