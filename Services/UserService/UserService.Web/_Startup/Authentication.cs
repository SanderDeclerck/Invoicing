using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UserService.Web.Startup;

public static class Authentication
{
    public static void AddCustomAuthentication(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Auth0Constants.AuthenticationScheme;
            })
            .AddAuth0WebAppAuthentication(options => {
                (options.Domain, 
                 options.ClientId, 
                 options.ClientSecret) = builder.Configuration.GetAuth0Settings();
                
                options.CallbackPath = new PathString("/signin-auth0");
            })
            .WithAccessToken(options => {
                options.Audience = "sd-invoice";
            });
    }
}
