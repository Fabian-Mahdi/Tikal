using Identity.Application.Identity.Commands.Login;

namespace Identity.Application.Tests.Identity.Commands.Login;

public static class LoginCommandTestCasesSource
{
    public static IEnumerable<LoginCommand> ValidTestCases()
    {
        yield return new LoginCommand("admin", "admin");
        yield return new LoginCommand("user", "password");
        yield return new LoginCommand("alskdfjalseikfasd", "asdo343flaieswfja2342");
    }

    public static IEnumerable<LoginCommand> InvalidTestCases()
    {
        yield return new LoginCommand("user", "");
        yield return new LoginCommand("", "password");
        yield return new LoginCommand("", "");
    }
}