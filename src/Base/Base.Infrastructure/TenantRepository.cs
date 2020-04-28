using System.Linq;
using Invoicing.Base.Ddd;
using Microsoft.AspNetCore.Http;

namespace Base.Infrastructure
{
    public abstract class TenantRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected TenantRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public abstract IUnitOfWork UnitOfWork { get; }

        protected string? TenantId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (user == null)
                {
                    return null;
                }

                var tenantClaim = user.Claims.FirstOrDefault(claim => claim.Type == "tenant_id");

                return tenantClaim?.Value ?? null;
            }
        }
    }
}