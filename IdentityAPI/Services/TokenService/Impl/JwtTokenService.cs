
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Services.TokenService.Impl;

public class JwtTokenService : ITokenService
{
    private readonly SecurityTokenHandler securityTokenHandler;

    public JwtTokenService(SecurityTokenHandler securityTokenHandler)
    {
        this.securityTokenHandler = securityTokenHandler;
    }

    public TokenPair GenerateTokenPair(Guid userId, string username)
    {
        throw new NotImplementedException();
    }
}