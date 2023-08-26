using AuthenticationService.Web.Data;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Web.Startup;

public static class Identity
{
    public static void AddCustomIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<User>(options => {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddAzureTableStores<ApplicationDbContext>(() => {
                return new IdentityConfiguration {
                    TablePrefix = "v3",
                    StorageConnectionString = builder.Configuration.GetConnectionString("Identity")
                };
            })
            .CreateAzureTablesIfNotExists<ApplicationDbContext>();
        

        builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationUserClaimsPrincipalFactory>();
    }
}

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public ApplicationUserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("TenantId", user.TenantId ?? string.Empty));
        return identity;
    }
}