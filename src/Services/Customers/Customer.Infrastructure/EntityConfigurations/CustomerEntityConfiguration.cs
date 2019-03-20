using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Customers.Infrastructure.EntityConfigurations
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", CustomerDbContext.Schema);

            builder.OwnsOne(c => c.BillingAddress, addressBuilder => addressBuilder.Ignore(a => a.Country));
            builder.OwnsOne(c => c.ShippingAddress, addressBuilder => addressBuilder.Ignore(a => a.Country));
        }
    }
}
