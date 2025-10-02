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
}