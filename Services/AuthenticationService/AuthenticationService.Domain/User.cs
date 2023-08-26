using ElCamino.AspNetCore.Identity.AzureTable.Model;

namespace AuthenticationService.Domain;

public class User : IdentityUser
{
    public string? TenantId { get; set; }

    public void UpdateTenant(string tenant)
    {
        TenantId = tenant;
    }
}
