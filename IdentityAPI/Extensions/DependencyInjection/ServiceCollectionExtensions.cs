using IdentityAPI.Services.TokenService;
using IdentityAPI.Services.TokenService.Impl;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAPI.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationDependencyGroup(this IServiceCollection services)
    {
        services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();

        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }
}