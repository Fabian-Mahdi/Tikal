namespace IdentityAPI.Tests.Data.Other;

public static class ValidPasswordSource
{
    public static IEnumerable<string> TestCases()
    {
        yield return "Password!1";
        yield return "asjZf%342dja&l23";
    }
}