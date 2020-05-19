using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Invoicing.Customers.Infrastructure;
using Microsoft.AspNetCore.Http;
using MediatR;
using FluentValidation.AspNetCore;

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

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5000";
                    options.Audience = "customer.api";
                    options.RequireHttpsMetadata = true;
                });

            services.AddCors(builder =>
            {
                builder.AddDefaultPolicy(options =>
                {
                    options.WithOrigins("https://localhost:5010").AllowAnyHeader().AllowCredentials();
                });
            });
            services.AddMediatR(typeof(Startup));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCustomersInfrastructure("mongodb://127.0.0.1:27017");
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
            });
        }
    }
}
