namespace IdentityAPI.Dtos;

public record TokenDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}