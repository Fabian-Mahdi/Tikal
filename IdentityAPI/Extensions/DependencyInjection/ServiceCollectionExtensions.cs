using IdentityAPI.Configuration;
using IdentityAPI.Database;
using IdentityAPI.Database.Repositories.UserRepository;
using IdentityAPI.Services.TokenService;
using IdentityAPI.Services.TokenService.Impl;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAPI.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Position));

        return services;
    }

    public static IServiceCollection AddAuthenticationDependencyGroup(this IServiceCollection services)
    {
        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();

        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<UnitOfWork>();

        return services;
    }
}