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
}