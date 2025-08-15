namespace IdentityAPI.Tests.Data.Other;

public static class InvalidPasswordSource
{
    public static IEnumerable<string> TestCases()
    {
        yield return "";
        yield return "short";
        yield return "password";
        yield return "NoSpecialCharacters123";
        yield return "ONLYCAPS123";
        yield return "faowefijsakldfjaosweilfjaoajiofe";
    }
}