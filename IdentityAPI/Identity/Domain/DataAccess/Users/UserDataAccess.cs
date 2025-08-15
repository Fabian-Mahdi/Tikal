using FluentResults;
using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.DataAccess.Users;

/// <summary>
///     The data access for all operations regarding users
/// </summary>
public interface UserDataAccess
{
    /// <summary>
    ///     Creates a new user with a given password
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="password">The password to assign</param>
    /// <returns>Void in case of success otherwise <see cref="UserError" /></returns>
    Task<Result> CreateUser(User user, string password);

    /// <summary>
    ///     Validates a password for a given user
    /// </summary>
    /// <param name="user">The user for which to validate the password</param>
    /// <param name="password">The password to validate</param>
    /// <returns>True if the password is valid, otherwise False</returns>
    Task<bool> ValidatePassword(User user, string password);

    /// <summary>
    ///     Retrieves a user with a given username
    /// </summary>
    /// <param name="username">The username for which to search for</param>
    /// <returns>The found user, null if no user is found</returns>
    Task<User?> FindByName(string username);
}