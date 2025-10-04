namespace Identity.Domain.Identity;

/// <summary>
///     Represents a registered user
/// </summary>
public class User
{
    /// <summary>
    ///     The unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The username used for login and display
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="User" /> with a given id and username
    /// </summary>
    /// <param name="id">The id of the <see cref="User" /></param>
    /// <param name="username">The username of the <see cref="User" /></param>
    public User(int id, string username)
    {
        Id = id;
        Username = username;
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="User" /> with a given username and no id
    /// </summary>
    /// <param name="username">The username of the <see cref="User" /></param>
    public User(string username)
    {
        Username = username;
    }
}