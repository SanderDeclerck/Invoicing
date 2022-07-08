namespace Invoicing.SharedKernel.Domain;

public abstract class EntityBase
{
    protected EntityBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}