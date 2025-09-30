using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.DataAccess;

/// <summary>
///     The repository for all operations regarding <see cref="User" />
/// </summary>
public interface UserRepository
{
    /// <summary>
    ///     Gets a <see cref="User" /> with a given username
    /// </summary>
    /// <param name="username">The username of the <see cref="User" /> to retrieve</param>
    /// <param name="cancellationToken">
    ///     <see cref="CancellationToken" />
    /// </param>
    /// <returns>
    ///     The <see cref="User" /> with the given username, null if no <see cref="User" /> with the given name could be
    ///     found
    /// </returns>
    Task<User?> FindByUsername(string username, CancellationToken cancellationToken);

    /// <summary>
    ///     Creates a given <see cref="User" /> with a given password
    /// </summary>
    /// <param name="user">The <see cref="User" /> to create</param>
    /// <param name="password">The password to assign to the <see cref="User" /></param>
    /// <param name="cancellationToken">
    ///     <see cref="CancellationToken" />
    /// </param>
    /// <returns>
    ///     The created <see cref="User" />, or <see cref="DuplicateUsername" /> if a user with the given username already
    ///     exists
    /// </returns>
    Task<OneOf<User, DuplicateUsername>> CreateUser(User user, string password, CancellationToken cancellationToken);
}