namespace IdentityAPI.Authentication.Domain.Models;

public class AccessToken
{
    public string Value { get; set; }

    public AccessToken(string value)
    {
        Value = value;
    }
}