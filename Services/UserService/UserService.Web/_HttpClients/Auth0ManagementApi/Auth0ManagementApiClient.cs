using System.Text.Json;

namespace UserService.Web.HttpClients.Auth0ManagementApi;

public class Auth0ManagementApiClient(HttpClient httpClient)
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task<User[]> GetUsersAsync()
    {
        var response = (await httpClient.GetAsync("api/v2/users")).EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User[]>(jsonSerializerOptions) ?? Array.Empty<User>();
    }
}