using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityAPI.Configuration;
using IdentityAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Services.TokenService.Impl;

public class JwtTokenService : ITokenService
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

    public TokenPair GenerateTokenPair(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.UserName!)
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

        TokenPair tokenPair = new()
        {
            AccessToken = securityTokenHandler.WriteToken(accessToken),
            RefreshToken = securityTokenHandler.WriteToken(refreshToken)
        };

        return tokenPair;
    }
}