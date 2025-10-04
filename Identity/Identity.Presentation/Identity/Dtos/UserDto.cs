namespace Identity.Presentation.Identity.Dtos;

/// <summary>
///     Holds data for a user
/// </summary>
/// <param name="id">The id of the user</param>
/// <param name="username">The username of the user</param>
public record UserDto(int id, string username);