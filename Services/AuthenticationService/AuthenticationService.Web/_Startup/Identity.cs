using ElCamino.AspNetCore.Identity.AzureTable.Model;
using AuthenticationService.Web.Data;

namespace AuthenticationService.Web.Startup;

public static class Identity
{
    public static void AddCustomIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddAzureTableStores<ApplicationDbContext>(() => {
                return new IdentityConfiguration {
                    TablePrefix = "v3",
                    StorageConnectionString = builder.Configuration.GetConnectionString("Identity")
                };
            })
            .CreateAzureTablesIfNotExists<ApplicationDbContext>();
    }
}
