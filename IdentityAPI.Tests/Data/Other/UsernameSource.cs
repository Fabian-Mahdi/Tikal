namespace IdentityAPI.Tests.Data.Other;

public static class UsernameSource
{
    public static IEnumerable<string> TestCases()
    {
        yield return "username";
        yield return "";
        yield return "apsoddifjvaweopfijffaomie";
    }
}