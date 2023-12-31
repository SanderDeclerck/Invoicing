using UserService.Web.HttpClients;
using UserService.Web.HttpClients.Auth0ManagementApi;

namespace UserService.Web.Startup;

public static class HttpClients
{
    public static void AddCustomHttpClients(this IHostApplicationBuilder builder)
    {
        var (domain, clientId, clientSecret) = builder.Configuration.GetAuth0Settings();

        const string Auth0ClientKey = "auth0-client";

        builder.Services.AddHttpClient(Auth0ClientKey, (provider, client) => {
            client.BaseAddress = new Uri($"https://{domain}/");
        });

        builder.Services.AddKeyedSingleton(Auth0ClientKey, (provider, _)
            => new ClientCredentialsTokenClient(provider.GetRequiredService<IHttpClientFactory>(),
                                                Auth0ClientKey,
                                                clientId,
                                                clientSecret,
                                                $"https://{domain}/api/v2/"));

        builder.Services
            .AddHttpClient<Auth0ManagementApiClient>((provider, client) => {
                client.BaseAddress = new Uri($"https://{domain}/");
            })
            .AddHttpMessageHandler(provider => {
                var clientCredentialsTokenClient = provider.GetRequiredKeyedService<ClientCredentialsTokenClient>(Auth0ClientKey);
                var handler = new ClientCredentialsHttpHandler(clientCredentialsTokenClient);
                return handler;
            });
    }
}
