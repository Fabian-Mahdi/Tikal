using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Identity.Presentation.Dtos;

/// <summary>
///     Holds all tokens returned when authenticating
/// </summary>
public class TokenDto
{
    /// <summary>
    ///     The short-lived access token
    /// </summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;
}