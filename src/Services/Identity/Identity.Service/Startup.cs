using Microsoft.AspNetCore.Builder;
using DbUp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Identity.Service.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.Service.Domain;
using IdentityServer4.Services;
using Identity.Service.Services;
using Identity.Service.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.Tasks;

namespace Identity.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var baseUrlConfiguration = Configuration.GetSection("BaseUrls").Get<BaseUrlConfiguration>();

            MigrateDatabase(connectionString);

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.GetClients(baseUrlConfiguration))
                .AddAspNetIdentity<ApplicationUser>();

            builder.Services.AddTransient<IProfileService, ProfileService>();

            builder.AddDeveloperSigningCredential();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
                app.Use(async (context, next) =>
                        {
                            // In production: always use https --> Set scheme in case tls is terminated by a reverse proxy
                            context.Request.Scheme = "https";
                            await next.Invoke();
                        });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void MigrateDatabase(string connectionString)
        {
            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(Startup).Assembly)
                .LogToConsole()
                .Build();

            upgrader.PerformUpgrade();
        }
    }
}