namespace IdentityAPI.Dtos;

public record UserDto
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
}