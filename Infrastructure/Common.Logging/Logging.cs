﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> configureLogger =>
            (context, loggerConfiguration) =>
            {
                var env=context.HostingEnvironment;
                loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application",env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName",env.EnvironmentName)     
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console();
                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                }
                // Writing to Elastic Search
                var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                if (!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUrl))
                        {
                            AutoRegisterTemplate=true,
                            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                            IndexFormat="EShopping-Logs-{0:yyyy.MM.dd}",
                            MinimumLogEventLevel=LogEventLevel.Debug
                        });
                }
            };


    }
}
