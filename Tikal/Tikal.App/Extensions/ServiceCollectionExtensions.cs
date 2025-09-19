using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Tikal.App.Configuration;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Core.DataAccess;
using Tikal.Infrastructure.Accounts;
using Tikal.Infrastructure.Accounts.Mappers;
using Tikal.Infrastructure.Database;

namespace Tikal.App.Extensions;

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
        string connectionString = builder.Configuration.GetConnectionString("TikalDatabase")!;

        services.AddDbContext<ApplicationDbContext>(optionsBuilder => { optionsBuilder.UseNpgsql(connectionString); });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UnitOfWork, ApplicationUnitOfWork>();

        services.AddScoped<AccountRepository, AccountDatabase>();
    }

    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<AccountMapper>();
    }

    public static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        JwtOptions jwtOptions = new();
        configuration.GetSection(JwtOptions.Position).Bind(jwtOptions);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                };
            });
    }

    public static void AddMandatoryAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build()
            );
    }
}