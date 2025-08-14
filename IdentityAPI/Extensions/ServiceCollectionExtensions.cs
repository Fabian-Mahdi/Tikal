using System.IdentityModel.Tokens.Jwt;
using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Authentication.Infrastructure.Identity;
using IdentityAPI.Authentication.Infrastructure.Mappers;
using IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;
using IdentityAPI.Authentication.Infrastructure.Services;
using IdentityAPI.Configuration;
using IdentityAPI.Database;
using IdentityAPI.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

namespace IdentityAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });

        services.AddExceptionHandler<ProblemExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddAuthenticationDependencyGroup(this IServiceCollection services)
    {
        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();

        services.AddScoped<UserDataAccess, IdentityUserService>();
        services.AddScoped<TokenDataAccess, JwtTokenService>();
        services.AddScoped<CredentialsDataAccess, IdentityCredentialsService>();

        services.AddScoped<IUserMapper, UserMapper>();

        services.AddScoped<RegisterUser>();
        services.AddScoped<LoginUser>();
        services.AddScoped<RefreshTokens>();

        return services;
    }

    public static IServiceCollection AddDevOpenTelemetry(this IServiceCollection services)
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

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("identitydb")!;

        services.AddDbContext<ApplicationDbContext>(optionsBuilder => { optionsBuilder.UseNpgsql(connectionString); });

        return services;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));

        return services;
    }

    public static IServiceCollection AddProdCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("production",
                builder => builder.WithOrigins("https://tikalonline.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        return services;
    }
}