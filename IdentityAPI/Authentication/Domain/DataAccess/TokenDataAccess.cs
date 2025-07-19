using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.DataAccess;

public interface TokenDataAccess
{
    (RefreshToken, AccessToken) GenerateTokenPair(User user);

    Task<bool> ValidateToken(string token);

    Task<T?> ExtractClaim<T>(string token, string claimName);
}