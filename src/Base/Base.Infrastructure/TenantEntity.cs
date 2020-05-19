using Invoicing.Base.Ddd;

namespace Base.Infrastructure
{
    public class TenantEntity<TEntity> where TEntity : Entity
    {
        public TEntity Entity { get; }
        public string TenantId { get; }

        public TenantEntity(TEntity entity, string tenantId)
        {
            Entity = entity;
            TenantId = tenantId;
        }
    }
}