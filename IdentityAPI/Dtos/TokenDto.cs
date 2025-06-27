namespace IdentityAPI.Dtos;

public record TokenDto
{
    public required string AccessToken { get; init; }
}