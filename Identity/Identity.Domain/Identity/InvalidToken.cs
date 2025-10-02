namespace Identity.Domain.Identity;

/// <summary>
///     Indicates that a given token was invalid
/// </summary>
public class InvalidToken
{
    /// <summary>
    ///     Gets the token which was the source of the error
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="InvalidToken" /> with a given token
    /// </summary>
    /// <param name="token">The token which was the source of the error</param>
    public InvalidToken(string token)
    {
        Token = token;
    }
}