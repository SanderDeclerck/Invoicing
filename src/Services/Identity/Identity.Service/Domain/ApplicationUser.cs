using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public ApplicationUser(string userName) : base(userName) { }

        public Tenant? Tenant { get; set; }
    }
}