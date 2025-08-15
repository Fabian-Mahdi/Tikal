using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Identity.Presentation.Dtos;

/// <summary>
///     Holds all data needed for a register operation
/// </summary>
public class RegisterDto
{
    /// <summary>
    ///     The username of the user to be registered
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    ///     The password of the user to be registered
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}