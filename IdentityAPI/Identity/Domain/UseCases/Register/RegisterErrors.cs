using FluentResults;

namespace IdentityAPI.Identity.Domain.UseCases.Register;

/// <summary>
///     The base class for all register errors
/// </summary>
public abstract class RegisterError : Error
{
}

/// <summary>
///     The username does not meet our standards
/// </summary>
public class InvalidUsername : RegisterError
{
    /// <summary>
    ///     Gets the username which is the source of the error
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="InvalidUsername" /> with a given username
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    public InvalidUsername(string username)
    {
        Username = username;
    }
}

/// <summary>
///     The password does not meet our standards
/// </summary>
public class InvalidPassword : RegisterError
{
}

/// <summary>
///     A user with the given name already exists
/// </summary>
public class DuplicateUserName : RegisterError
{
    /// <summary>
    ///     Gets the username which is the source of the error
    /// </summary>
    public string Username { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="DuplicateUserName" /> with a given username
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    public DuplicateUserName(string username)
    {
        Username = username;
    }
}