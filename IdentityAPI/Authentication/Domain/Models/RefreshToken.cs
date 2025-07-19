namespace IdentityAPI.Authentication.Domain.Models;

public class RefreshToken
{
    public string Value { get; set; }

    public RefreshToken(string value)
    {
        Value = value;
    }
}