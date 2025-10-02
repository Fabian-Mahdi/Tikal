using System.Security.Claims;
using System.Text;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using Identity.Infrastructure.Identity.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Identity;

public class JwtTokenDatabase : TokenRepository
{
    private readonly JwtOptions options;

    private readonly SecurityTokenHandler securityTokenHandler;

    private readonly TokenValidationParameters tokenValidationParameters;

    private readonly byte[] signingKey;

    public JwtTokenDatabase(IOptions<JwtOptions> options, SecurityTokenHandler securityTokenHandler)
    {
        this.securityTokenHandler = securityTokenHandler;
        this.options = options.Value;

        signingKey = Encoding.UTF8.GetBytes(this.options.SigningKey);

        tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = this.options.Issuer,
            ValidateAudience = true,
            ValidAudience = this.options.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKey),
            ValidateLifetime = true
        };
    }

    public TokenPair GenerateTokenPair(User user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username)
        ];

        DateTime now = DateTime.UtcNow;

        SecurityTokenDescriptor accessTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddSeconds(options.AccessTokenExpiration),
            IssuedAt = now,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        SecurityTokenDescriptor refreshTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddSeconds(options.RefreshTokenExpiration),
            IssuedAt = now,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        SecurityToken accessToken = securityTokenHandler.CreateToken(accessTokenDescriptor);
        SecurityToken refreshToken = securityTokenHandler.CreateToken(refreshTokenDescriptor);

        TokenPair tokenPair = new(
            securityTokenHandler.WriteToken(accessToken),
            securityTokenHandler.WriteToken(refreshToken)
        );

        return tokenPair;
    }

    public async Task<bool> ValidateToken(string token)
    {
        TokenValidationResult result = await securityTokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

        return result.IsValid;
    }

    public async Task<T?> ExtractClaim<T>(string token, string claimName)
    {
        TokenValidationResult result = await securityTokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

        if (result.Claims.TryGetValue(claimName, out object? claim))
        {
            return (T)claim;
        }

        return default;
    }
}