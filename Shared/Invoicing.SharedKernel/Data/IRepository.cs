using Invoicing.SharedKernel.Domain;

namespace Invoicing.SharedKernel.Data;

public interface IRepository<T> where T : IAggregateRoot { }
