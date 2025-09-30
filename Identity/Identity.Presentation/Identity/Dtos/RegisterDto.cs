namespace Identity.Presentation.Identity.Dtos;

/// <summary>
///     Holds all data needed for a register operation
/// </summary>
/// <param name="username">The name of the user to be registered</param>
/// <param name="password">The password of the user to be registered</param>
public record RegisterDto(string username, string password);