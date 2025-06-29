using Azure.Monitor.OpenTelemetry.AspNetCore;
using IdentityAPI.Configuration;
using IdentityAPI.Database;
using IdentityAPI.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;

namespace IdentityAPI.Extensions;

public static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseOptions options = new();
        configuration.GetSection(DatabaseOptions.Position).Bind(options);

        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(
                $"Server={options.Host};" +
                $"Port={options.Port};" +
                $"Database={options.DatabaseName};" +
                $"User ID={options.Username};" +
                $"Password={options.Password};"
                );
        });

        return services;
    }
}