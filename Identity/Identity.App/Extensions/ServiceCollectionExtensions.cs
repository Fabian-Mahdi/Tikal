using System.IdentityModel.Tokens.Jwt;
using Identity.App.Configuration;
using Identity.Application;
using Identity.Application.Core.Pipelines;
using Identity.Application.Identity.DataAccess;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Identity;
using Identity.Infrastructure.Identity.Configuration;
using Identity.Infrastructure.Identity.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;

namespace Identity.App.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDevOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter();
            })
            .WithLogging(logging =>
            {
                logging
                    .AddOtlpExporter();
            });
    }

    public static void AddDevDbContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("IdentityDatabase")!;

        services.AddDbContext<ApplicationDbContext>(optionsBuilder => { optionsBuilder.UseNpgsql(connectionString); });
    }

    public static void AddProdOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSentry();
            });
    }

    public static void AddProductionCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "production",
                builder =>
                    builder.WithOrigins("https://tikalonline.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
            );
        });
    }

    public static void AddProdDbContext(this IServiceCollection services, IConfiguration configuration)
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
    }

    public static void AddDevMediatr(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));
    }

    public static void AddProdMediatr(this IServiceCollection services, IConfiguration configuration)
    {
        MediatrOptions options = new();
        configuration.GetSection(MediatrOptions.Position).Bind(options);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            config.LicenseKey = options.LicenseKey;
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();

        services.AddScoped<UserRepository, UserDatabase>();

        services.AddScoped<TokenRepository, JwtTokenDatabase>();
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<UserMapper>();
    }

    public static void AddPipelines(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
    }

    public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
    }
}