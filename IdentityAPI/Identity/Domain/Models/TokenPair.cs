namespace IdentityAPI.Identity.Domain.Models;

/// <summary>
///     A pair of tokens in string format
/// </summary>
public class TokenPair
{
    /// <summary>
    ///     Gets the access token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets the refresh token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    ///     Initializes a new instance of <see cref="TokenPair" /> with the given tokens
    /// </summary>
    /// <param name="accessToken">The access token of the pair</param>
    /// <param name="refreshToken">The refresh token of the pair</param>
    public TokenPair(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}