using ElCamino.AspNetCore.Identity.AzureTable;
using ElCamino.AspNetCore.Identity.AzureTable.Model;

namespace AuthenticationService.Web.Data;

public class ApplicationDbContext : IdentityCloudContext
{
    public ApplicationDbContext(IdentityConfiguration configuration)
        : base(configuration)
    {
    }
}
