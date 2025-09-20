namespace Tikal.Domain.Accounts;

/// <summary>
///     Represents an account of a player
/// </summary>
public class Account
{
    /// <summary>
    ///     The unique identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     The display name shown to other players
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="Account" /> with a given id and name
    /// </summary>
    /// <param name="id">The id of the account</param>
    /// <param name="name">The name of the account</param>
    public Account(string id, string name)
    {
        Id = id;
        Name = name;
    }
}