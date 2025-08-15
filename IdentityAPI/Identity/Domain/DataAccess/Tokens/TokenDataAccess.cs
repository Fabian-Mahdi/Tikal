using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.DataAccess.Tokens;

/// <summary>
///     The data access for all operations regarding tokens
/// </summary>
public interface TokenDataAccess
{
    /// <summary>
    ///     Generates a pair of tokens for a given user
    /// </summary>
    /// <param name="user">The user for which to generate tokens for</param>
    /// <returns>A token pair consisting of access and refresh token</returns>
    TokenPair GenerateTokenPair(User user);

    /// <summary>
    ///     Validates a given token
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>True if the token is valid, otherwise false</returns>
    /// <remarks>Validates Issuer, Audience, SigningKey and Lifetime</remarks>
    Task<bool> ValidateToken(string token);

    /// <summary>
    ///     Extracts a claim with the given name from the given token
    /// </summary>
    /// <param name="token">The token from which to extract the claim</param>
    /// <param name="claimName">The name of the claim to be extracted</param>
    /// <typeparam name="T">The type of the claim</typeparam>
    /// <returns>The extracted claim of type T on success, otherwise the default value of T</returns>
    Task<T?> ExtractClaim<T>(string token, string claimName);
}