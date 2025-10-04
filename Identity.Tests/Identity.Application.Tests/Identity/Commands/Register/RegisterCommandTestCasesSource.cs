using Identity.Application.Identity.Commands.Register;

namespace Identity.Application.Tests.Identity.Commands.Register;

public static class RegisterCommandTestCasesSource
{
    public static IEnumerable<RegisterCommand> ValidTestCases()
    {
        yield return new RegisterCommand("username", "Password1!");
        yield return new RegisterCommand("testing", "SuperSecure!123");
        yield return new RegisterCommand("jaksdflaiea123432", "skdfJa234jfAo23$§$§()§$");
    }

    public static IEnumerable<RegisterCommand> InvalidTestCases()
    {
        yield return new RegisterCommand("", "Password1!");
        yield return new RegisterCommand("username", "");
        yield return new RegisterCommand("tooooooooooooooooooLoooooooooooooooooooong", "invalid");
    }
}