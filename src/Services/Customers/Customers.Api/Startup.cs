using AutoMapper;
using Customers.Api.Application.Queries;
using FluentValidation.AspNetCore;
using Invoicing.Customers.Domain.CustomerAggregate;
using Invoicing.Customers.Infrastructure;
using Invoicing.Customers.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Customers.Api
{
    public class Startup
    {
        private const string ReactOrigin = "_myReactApp";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var myConnString = "Data Source=E:\\invoicing.db";

            services.AddScoped<ICustomerQueries, CustomerQueries>(provider => new CustomerQueries(myConnString));
            services.AddDbContext<CustomerDbContext>(builder => builder.UseSqlite(myConnString));
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddCors(opts => opts.AddPolicy(ReactOrigin, builder => builder.WithOrigins("https://localhost:44347")));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Customers.Api",
                    Version = "v1",
                    Description = "The customers service",
                    TermsOfService = "Terms of Service"
                });

                options.CustomSchemaIds(x => x.FullName);
            });

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(ReactOrigin);

            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers.Api"));
        }
    }
}
