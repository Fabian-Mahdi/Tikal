using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Authentication.Infrastructure.Services;

public class JwtTokenService : TokenDataAccess
{
    private readonly JwtOptions options;

    private readonly SecurityTokenHandler securityTokenHandler;

    private readonly byte[] signingKey;

    public JwtTokenService(SecurityTokenHandler securityTokenHandler, IOptions<JwtOptions> options)
    {
        this.securityTokenHandler = securityTokenHandler;
        this.options = options.Value;

        signingKey = Encoding.UTF8.GetBytes(this.options.SigningKey);
    }

    public (RefreshToken, AccessToken) GenerateTokenPair(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.Username)
        ];

        SecurityTokenDescriptor accessTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(options.AccessTokenExpiration),
            IssuedAt = DateTime.UtcNow,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityTokenDescriptor refreshTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(options.RefreshTokenExpiration),
            IssuedAt = DateTime.UtcNow,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken accessToken = securityTokenHandler.CreateToken(accessTokenDescriptor);
        SecurityToken refreshToken = securityTokenHandler.CreateToken(refreshTokenDescriptor);

        return (
            new RefreshToken(securityTokenHandler.WriteToken(accessToken)),
            new AccessToken(securityTokenHandler.WriteToken(refreshToken))
        );
    }
}