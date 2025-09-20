namespace Tikal.Domain.Accounts;

/// <summary>
///     Indicates that an <see cref="Account" /> with a given Id already exists
/// </summary>
public class DuplicateAccountId
{
    /// <summary>
    ///     Gets the Id which was the source of the error
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="DuplicateAccountId" /> with a given Id
    /// </summary>
    /// <param name="id">The Id which was the source of the error</param>
    public DuplicateAccountId(string id)
    {
        Id = id;
    }
}