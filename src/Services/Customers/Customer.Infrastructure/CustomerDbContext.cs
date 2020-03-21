using System.Threading;
using System.Threading.Tasks;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Customers.Infrastructure
{
    public class CustomerDbContext : DbContext, IUnitOfWork
    {
        public const string Schema = "Customer";
        public DbSet<Customer> Customers { get; set; } = null!;

        public CustomerDbContext(DbContextOptions<CustomerDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CustomerEntityConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
