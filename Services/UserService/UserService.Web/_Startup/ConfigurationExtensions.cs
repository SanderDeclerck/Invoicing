namespace UserService.Web.Startup;

public static class ConfigurationExtensions
{
    public static (string domain, string clientId, string clientSecret) GetAuth0Settings(this IConfiguration coonfiguration)
    {
        var auth0Settings = coonfiguration.GetSection("Auth0");

        var domain = auth0Settings["Domain"];
        var clientId = auth0Settings["ClientId"];
        var clientSecret = auth0Settings["ClientSecret"];

        if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("Auth0 settings are not configured correctly.");
        }

        return (domain, clientId, clientSecret);
    }
}
