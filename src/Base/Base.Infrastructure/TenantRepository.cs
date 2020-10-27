using System.Linq;
using Invoicing.Base.Ddd;
using Microsoft.AspNetCore.Http;

namespace Base.Infrastructure
{
    public abstract class TenantRepository<T> : IRepository<T> where T : Entity, IAggregateRoot
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected TenantRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public abstract IUnitOfWork UnitOfWork { get; }

        protected string TenantId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext.User;

                var tenantClaim = user?.Claims.FirstOrDefault(claim => claim.Type == "tenant_id");

                return tenantClaim?.Value ?? throw new System.Exception("It is not allowed to access tenant-related data when not authenticated to a tenant");
            }
        }

        protected TenantEntity<T> WrapForTenant(T entity) => new TenantEntity<T>(entity, TenantId);
    }
}