using Catalog.Application;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            services.AddApplicationDepedency();
            services.AddHealthChecks()
                .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"], "Catalog Mongo Db Health Checks"
                , Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Catalog.API", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));
            //services.AddMediatR(typeof(CreateProductCommandHandler).GetTypeInfo().Assembly);
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypeRepository, ProductRepository>();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }


    }
}
