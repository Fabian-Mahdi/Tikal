using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Identity.Presentation.Dtos;

/// <summary>
///     Holds all data needed for a login operation
/// </summary>
public class LoginDto
{
    /// <summary>
    ///     The username for the login attempt
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    ///     The password for the login attempt
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}