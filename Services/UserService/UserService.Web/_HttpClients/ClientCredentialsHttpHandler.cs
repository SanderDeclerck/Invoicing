using System.Net.Http.Headers;

namespace UserService.Web.HttpClients;

public class ClientCredentialsHttpHandler(ClientCredentialsTokenClient clientCredentialsTokenClient) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await clientCredentialsTokenClient.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}
