using Microsoft.AspNetCore.Mvc.RazorPages;
using UserService.Web.HttpClients.Auth0ManagementApi;

namespace UserService.Web.Pages;

public class IndexModel(Auth0ManagementApiClient auth0ManagementApiClient) : PageModel
{
    public List<UserModel> Users { get; private set; } = new();

    public async Task OnGetAsync()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var users = await auth0ManagementApiClient.GetUsersAsync();

            Users = users.Select(MapToModel).ToList();
        }
    }

    public UserModel MapToModel(User user)
    {
        string? tenant = null;
        user.AppMetadata?.TryGetValue("tenant", out tenant);

        return new UserModel(user.Email, user.Name, user.Nickname, user.Picture, user.UserId, tenant ?? "Unknown");
    }

    public record UserModel(string? Email, string? Name, string? Nickname, string? Picture, string? UserId, string Tenant);
}
