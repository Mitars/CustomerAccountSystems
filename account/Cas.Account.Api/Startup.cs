using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cas.Account.Api.Middleware;
using Cas.Account.Core;
using Cas.Account.Core.Interfaces;
using Cas.Account.DataAccess;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cas.Account.Api
{
    /// <summary>
    /// The API startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the development services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("cas-account"));

            this.ConfigureServices(services);
        }

        /// <summary>
        /// Configures the production services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseMySql(
                this.Configuration.GetConnectionString("DefaultConnection"),
                MySqlServerVersion.LatestSupportedServerVersion));

            this.ConfigureServices(services);
        }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime.
        /// </summary>
        /// <param name="services">The services collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Create.Handler).GetTypeInfo().Assembly);
            services.AddScoped<Mediator>();
            services.AddScoped<ICustomerAccessor, CustomerAccessor>();
            services.AddScoped<IAccountAccessor, AccountAccessor>();
            services.AddScoped<ITransactionAccessor, TransactionAccessor>();

            // TODO: Configure
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Create.Handler>());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cas.Account.Api", Version = "v1" });
            });
        }

        /// <summary>
        /// Configures the specified application.
        /// This method gets called by the runtime.
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cas.Account.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
