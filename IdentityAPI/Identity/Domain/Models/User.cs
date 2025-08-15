namespace IdentityAPI.Identity.Domain.Models;

/// <summary>
///     Represents a user in our system
/// </summary>
public class User
{
    /// <summary>
    ///     Gets or sets the identifier for this user
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     Gets or sets the username for this user
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Gets all roles associated with this user
    /// </summary>
    public List<Role> Roles { get; }

    /// <summary>
    ///     Initializes a new instance of <see cref="User" /> with a random id
    /// </summary>
    /// <param name="username">The users name</param>
    /// <remarks>
    ///     The Id property is initialized as a new GUID string value
    /// </remarks>
    public User(string username)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        Roles = [];
    }

    /// <summary>
    ///     Initialized a new instance of <see cref="User" /> with a given id
    /// </summary>
    /// <param name="username">The users name</param>
    /// <param name="id">the users id</param>
    public User(string username, string id)
    {
        Id = id;
        Username = username;
        Roles = [];
    }
}