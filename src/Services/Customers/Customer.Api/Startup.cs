using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Invoicing.Customers.Infrastructure;
using Microsoft.AspNetCore.Http;
using MediatR;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace Customer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());

            var identityServiceUrl = Configuration.GetSection(ConfigurationPath.Combine("BaseUrls", "IdentityService")).Get<string>();
            var frontendUrl = Configuration.GetSection(ConfigurationPath.Combine("BaseUrls", "Frontend")).Get<string>();
            var mongoConnectionString = GetMongoConnectionStringWithIp(Configuration.GetConnectionString("DefaultConnection"));

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = identityServiceUrl;
                    options.Audience = "customer.api";
                    options.RequireHttpsMetadata = true;
                });

            services.AddCors(builder =>
            {
                builder.AddDefaultPolicy(options =>
                {
                    options.WithOrigins(frontendUrl).AllowAnyHeader().AllowCredentials();
                });
            });
            services.AddMediatR(typeof(Startup));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCustomersInfrastructure(mongoConnectionString);

            services.AddHealthChecks()
                .AddMongoDb(mongoConnectionString)
                .AddIdentityServer(new Uri(identityServiceUrl));

            services.AddHealthChecksUI(setup => setup.AddHealthCheckEndpoint("Customer api", "/health")).AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
            });
        }

        private string GetMongoConnectionStringWithIp(string connectionString)
        {
            var builder = new MongoUrlBuilder(connectionString);
            var servers = new List<MongoServerAddress>();
            foreach (var server in builder.Servers)
            {
                var address = Dns.GetHostAddresses(server.Host).FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                if (address == null)
                {
                    throw new Exception($"Could not resolve address for {server.Host}");
                }

                servers.Add(new MongoServerAddress(address.ToString(), server.Port));
            }
            builder.Servers = servers;
            return builder.ToMongoUrl().ToString();
        }
    }
}
