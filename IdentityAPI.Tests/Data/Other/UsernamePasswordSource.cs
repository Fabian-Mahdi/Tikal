namespace IdentityAPI.Tests.Data.Other;

public static class UsernamePasswordSource
{
    public static IEnumerable<string[]> TestCases()
    {
        yield return ["username", "password"];
        yield return ["", ""];
        yield return ["asdioufhw9puihed", "apsdouihawoiufnpaoisdhj"];
    }
}