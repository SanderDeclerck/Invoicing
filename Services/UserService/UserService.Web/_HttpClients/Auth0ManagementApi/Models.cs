namespace UserService.Web.HttpClients.Auth0ManagementApi;

public class User
{
    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Nickname { get; set; }

    public string? Picture { get; set; }

    public string? UserId { get; set; }

    public IDictionary<string, string?>? AppMetadata { get; set; }
}
