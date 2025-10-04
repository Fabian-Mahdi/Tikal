using Identity.Domain.Identity;

namespace Identity.Application.Identity.DataAccess;

/// <summary>
///     The repository for all operations regarding access / refresh tokens
/// </summary>
public interface TokenRepository
{
    /// <summary>
    ///     Generates a <see cref="TokenPair" /> for a given <see cref="User" />
    /// </summary>
    /// <param name="user">The <see cref="User" /> for which to generate a <see cref="TokenPair" /></param>
    /// <returns>A <see cref="TokenPair" /> for a <see cref="User" /></returns>
    TokenPair GenerateTokenPair(User user);

    /// <summary>
    ///     Validates issuer, audience, expiration and signature of a given token
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>True if the token is valid, otherwise false</returns>
    Task<bool> ValidateToken(string token);

    /// <summary>
    ///     Extracts a claim with the given name from the given token
    /// </summary>
    /// <param name="token">The token from which to extract the claim</param>
    /// <param name="claimName">The name of the claim to be extracted</param>
    /// <typeparam name="T">The data type of the claim</typeparam>
    /// <returns>The extracted claim of type T on success, otherwise the default value of T</returns>
    Task<T?> ExtractClaim<T>(string token, string claimName);
}