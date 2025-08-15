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
}