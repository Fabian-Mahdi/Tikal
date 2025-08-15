using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Identity.Presentation.Dtos;

/// <summary>
///     Holds a pair of tokens, consisting of an access and a refresh token
/// </summary>
public class TokenDto
{
    /// <summary>
    ///     The access token of the pair
    /// </summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     the refresh token of the pair
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}