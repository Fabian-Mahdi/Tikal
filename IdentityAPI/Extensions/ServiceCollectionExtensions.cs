﻿using System.IdentityModel.Tokens.Jwt;
using Azure.Monitor.OpenTelemetry.AspNetCore;
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

    public static IServiceCollection AddAzureOpenTelemetry(this IServiceCollection services,
        IConfiguration configuration)
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

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));

        return services;
    }
}