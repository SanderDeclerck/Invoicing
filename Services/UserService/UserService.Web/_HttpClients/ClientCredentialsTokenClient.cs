using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace UserService.Web.HttpClients;

public class ClientCredentialsTokenClient(IHttpClientFactory httpClientFactory, string httpClientName, string clientId, string clientSecret, string? audience = null, string? scope = null)
{
    private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    private string? cachedToken = null;
    private DateTimeOffset cachedTokenExpiration = DateTimeOffset.MinValue;
    private readonly TimeSpan tokenExpirationBuffer = TimeSpan.FromMinutes(5);

    public async Task<string> GetTokenAsync()
    {
        if (IsTokenValid())
        {
            return cachedToken;
        }

        await RefreshToken();

        if (string.IsNullOrEmpty(cachedToken))
        {
            throw new Exception("Unable to retrieve access token.");
        }

        return cachedToken;
    }

    [MemberNotNullWhen(true, nameof(cachedToken))]
    private bool IsTokenValid()
    {
        if (string.IsNullOrEmpty(cachedToken))
        {
            return false;
        }

        return cachedTokenExpiration > DateTimeOffset.UtcNow.Add(tokenExpirationBuffer);
    }

    private async Task RefreshToken()
    {
        try
        {
            await semaphoreSlim.WaitAsync();

            if (IsTokenValid())
            {
                return;
            }

            var (token, expiration) = await GetToken();

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Unable to retrieve access token.");
            }

            cachedToken = token;
            cachedTokenExpiration = expiration;
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task<(string? accessToken, DateTimeOffset expiration)> GetToken()
    {
        var client = httpClientFactory.CreateClient(httpClientName);

        var response = await client.SendAsync(CreateRequest());
        response.EnsureSuccessStatusCode();

        return GetTokenFromResponse(response);
    }

    private (string? accessToken, DateTimeOffset expiration) GetTokenFromResponse(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(content);

        var accessToken = tokenResponse.GetProperty("access_token").GetString();
        var expiresIn = tokenResponse.GetProperty("expires_in").GetInt32();

        return (accessToken, DateTimeOffset.UtcNow.AddSeconds(expiresIn));
    }

    private HttpRequestMessage CreateRequest()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token")
        {
            Content = new FormUrlEncodedContent(CreateFormData())
        };

        return request;
    }

    private IDictionary<string, string> CreateFormData()
    {
        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
        };

        if (!string.IsNullOrEmpty(audience))
        {
            formData["audience"] = audience;
        }

        if (!string.IsNullOrEmpty(scope))
        {
            formData["scope"] = scope;
        }

        return formData;
    }
}