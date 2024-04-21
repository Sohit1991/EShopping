using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Swagger
{

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this._provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var descr in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(descr.GroupName, ProvideApiInfo(descr));
            }
        }

        private OpenApiInfo ProvideApiInfo(ApiVersionDescription descr)
        {
            var info = new OpenApiInfo()
            {
                Title = "Basket API Microservice",
                Version = descr.ApiVersion.ToString(),
                Description = "Fetched details about Basket",
                Contact = new OpenApiContact() { Name = "Sohit", Email = "sohitkhanchi@gmail.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri ( "https://opensource.org/licenses/MIT" ) }
            };
            if (descr.IsDeprecated)
            {
                info.Description += "API version has been deprected";
            }
            return info;
        }
    }
}
