using System.Collections.Generic;
using IdentityServer4.Models;

namespace Identity.Service.Configuration
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("tenant", new [] { "tenant_id", "tenant_name" })
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("customer.api", "Customer Api")
            };

        public static IEnumerable<Client> GetClients(BaseUrlConfiguration baseUrls) =>
            new Client[]
            {
                new Client
                {
                    ClientId = "invoicing.frontend",
                    ClientName = "Invoicing frontend",
                    ClientUri = baseUrls.Frontend,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,

                    RedirectUris =
                    {
                        $"{baseUrls.Frontend}/auth/callback",
                    },

                    PostLogoutRedirectUris = { baseUrls.Frontend },
                    AllowedCorsOrigins = { baseUrls.Frontend },

                    AllowedScopes = { "openid", "profile", "customer.api" }
                }
            };
    }
}
