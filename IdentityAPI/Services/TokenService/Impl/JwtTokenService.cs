using IdentityAPI.Configuration;
using IdentityAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAPI.Services.TokenService.Impl;

public class JwtTokenService : ITokenService
{
    private readonly SecurityTokenHandler securityTokenHandler;
    private readonly JwtOptions options;
    private readonly byte[] secret = "WillBeStoresSecurelyInTheNearFuture"u8.ToArray();

    public JwtTokenService(SecurityTokenHandler securityTokenHandler,
                           IOptions<JwtOptions> options)
    {
        this.securityTokenHandler = securityTokenHandler;
        this.options = options.Value;
    }

    public TokenPair GenerateTokenPair(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username),
        ];

        SecurityTokenDescriptor accessTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(options.AccessTokenExpiration),
            IssuedAt = DateTime.UtcNow,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityTokenDescriptor refreshTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(options.RefreshTokenExpiration),
            IssuedAt = DateTime.UtcNow,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken accessToken = securityTokenHandler.CreateToken(accessTokenDescriptor);
        SecurityToken refreshToken = securityTokenHandler.CreateToken(refreshTokenDescriptor);

        TokenPair tokenPair = new()
        {
            AccessToken = securityTokenHandler.WriteToken(accessToken),
            RefreshToken = securityTokenHandler.WriteToken(refreshToken),
        };

        return tokenPair;
    }
}