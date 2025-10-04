namespace Identity.Presentation.Identity.Dtos;

/// <summary>
///     Holds the token after a successful refresh or login operation
/// </summary>
/// <param name="accessToken">The access token used to authenticate in other parts of the system</param>
public record TokenDto(string accessToken);