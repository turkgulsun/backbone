using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Core.Infrastructure.Dependencies;

public static class SerilogDependencies
{
    public static IServiceCollection AddSerilogAndElasticDependencies(this IServiceCollection services)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true)
            .Build();
        
        Log.Logger= new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo. Debug ()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(services,configuration,environment))
            .Enrich.WithProperty ("Environment", environment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return services;
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(this IServiceCollection services,IConfiguration configuration, string environment)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"] ?? string.Empty))
        {
            AutoRegisterTemplate = true,
            IndexFormat =
                $"{Assembly.GetEntryAssembly()?.GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}