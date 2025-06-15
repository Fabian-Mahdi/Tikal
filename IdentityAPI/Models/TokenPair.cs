namespace IdentityAPI.Models;

public record TokenPair
{
    public required string RefreshToken { get; init; }
    public required string AccessToken { get; init; }
}