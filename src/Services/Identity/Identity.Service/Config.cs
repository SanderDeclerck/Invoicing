using System.Collections.Generic;
using IdentityServer4.Models;

namespace Identity.Service
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

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "invoicing.test",
                    ClientName = "Invoicing test cliet",
                    ClientUri = "https://localhost:5010",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,

                    RedirectUris =
                    {
                        "https://localhost:5010/callback.html",
                    },

                    PostLogoutRedirectUris = { "https://localhost:5010" },
                    AllowedCorsOrigins = { "https://localhost:5010" },

                    AllowedScopes = { "openid", "profile", "customer.api" }
                }
            };
    }
}
