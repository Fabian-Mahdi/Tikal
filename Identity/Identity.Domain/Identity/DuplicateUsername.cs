namespace Identity.Domain.Identity;

/// <summary>
///     Indicates that a <see cref="User" /> with a given name already exists
/// </summary>
public class DuplicateUsername
{
    /// <summary>
    ///     Gets the username which was the source of the error
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="DuplicateUsername" /> with a given username
    /// </summary>
    /// <param name="username">The username which was the source of the error</param>
    public DuplicateUsername(string username)
    {
        Username = username;
    }
}