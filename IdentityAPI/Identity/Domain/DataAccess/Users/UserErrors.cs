using FluentResults;

namespace IdentityAPI.Identity.Domain.DataAccess.Users;

/// <summary>
///     The base class for all user data access related errors
/// </summary>
public abstract class UserError : Error
{
}

/// <summary>
///     A user with the given name already exists
/// </summary>
public class UsernameUniqueConstraint : UserError
{
    /// <summary>
    ///     Gets the username which is the source of the error
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="UsernameUniqueConstraint" /> with a given username
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    public UsernameUniqueConstraint(string username)
    {
        Username = username;
    }
}