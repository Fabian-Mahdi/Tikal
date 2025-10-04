namespace Identity.Presentation.Identity.Dtos;

/// <summary>
///     Holds all data needed for a login operation
/// </summary>
/// <param name="username">The username of the given credentials</param>
/// <param name="password">The password of the given credentials</param>
public record LoginDto(string username, string password);