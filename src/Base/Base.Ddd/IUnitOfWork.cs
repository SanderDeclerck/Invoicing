using System;
using System.Threading;
using System.Threading.Tasks;

namespace Invoicing.Base.Ddd
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
