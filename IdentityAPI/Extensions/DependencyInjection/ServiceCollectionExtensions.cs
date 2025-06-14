using IdentityAPI.Services.TokenService;
using IdentityAPI.Services.TokenService.Impl;

namespace IdentityAPI.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }
}