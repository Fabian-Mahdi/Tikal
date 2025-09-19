namespace Tikal.App.Configuration;

public class JwtOptions
{
    public const string Position = "Jwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string SigningKey { get; set; } = string.Empty;
}