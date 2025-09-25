using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;
using Tikal.App.Configuration;
using Tikal.Application;
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

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        await context.HttpContext.WriteProblem(
                            StatusCodes.Status401Unauthorized,
                            "Unauthorized",
                            "Authentication is required to access this resource."
                        );
                    },
                    OnForbidden = async context =>
                    {
                        await context.HttpContext.WriteProblem(
                            StatusCodes.Status403Forbidden,
                            "Forbidden",
                            "Authentication is required to access this resource."
                        );
                    }
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
}