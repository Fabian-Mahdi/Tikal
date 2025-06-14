
using IdentityAPI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Services.TokenService.Impl;

public class JwtTokenService : ITokenService
{
    private readonly SecurityTokenHandler securityTokenHandler;

    private readonly JwtOptions options;

    public JwtTokenService(SecurityTokenHandler securityTokenHandler,
                           IOptions<JwtOptions> options)
    {
        this.securityTokenHandler = securityTokenHandler;
        this.options = options.Value;
    }

    public TokenPair GenerateTokenPair(Guid userId, string username)
    {
        throw new NotImplementedException();
    }
}