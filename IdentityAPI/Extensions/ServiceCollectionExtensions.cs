using Azure.Monitor.OpenTelemetry.AspNetCore;
using IdentityAPI.Configuration;
using IdentityAPI.ErrorHandling;
using OpenTelemetry.Resources;

namespace IdentityAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Position));

        return services;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });

        services.AddExceptionHandler<ProblemExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddAzureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry().UseAzureMonitor(options =>
        {
            options.ConnectionString = configuration.GetValue<string>("AzureInsightsConnectionString");
        }).ConfigureResource(resourceBuilder =>
        {
            resourceBuilder.Clear();
            resourceBuilder.AddService("IdentityApi");
        });

        return services;
    }
}