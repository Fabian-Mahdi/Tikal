namespace IdentityAPI.Dtos;

public record RegisterDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}