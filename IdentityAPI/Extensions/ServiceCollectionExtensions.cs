using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using IdentityAPI.Configuration;
using IdentityAPI.Database;
using IdentityAPI.Identity.Domain.DataAccess.Tokens;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Login;
using IdentityAPI.Identity.Domain.UseCases.Refresh;
using IdentityAPI.Identity.Domain.UseCases.Register;
using IdentityAPI.Identity.Domain.Validators;
using IdentityAPI.Identity.Infrastructure.Database;
using IdentityAPI.Identity.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;

namespace IdentityAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });

        return services;
    }

    public static IServiceCollection AddAuthenticationDependencyGroup(this IServiceCollection services)
    {
        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();

        // Identity

        // data access
        services.AddScoped<UserDataAccess, IdentityUserDatabase>();
        services.AddScoped<TokenDataAccess, JwtTokenDatabase>();

        // mappers
        services.AddScoped<UserMapper, UserMapperImpl>();

        // validators
        services.AddScoped<IValidator<User>, UserValidator>();
        services.AddScoped<IValidator<string>, PasswordValidator>();

        // use cases
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

    public static IServiceCollection AddProdOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSentry();
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