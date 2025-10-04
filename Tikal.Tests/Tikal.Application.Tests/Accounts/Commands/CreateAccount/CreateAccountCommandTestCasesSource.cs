using Tikal.Application.Accounts.Commands.CreateAccount;

namespace Tikal.Application.Tests.Accounts.Commands.CreateAccount;

public static class CreateAccountCommandTestCasesSource
{
    public static IEnumerable<CreateAccountCommand> ValidTestCases()
    {
        yield return new CreateAccountCommand(
            1,
            "name"
        );
        yield return new CreateAccountCommand(
            2,
            "20348290348"
        );
        yield return new CreateAccountCommand(
            3,
            "ajskdlföaieaslldf"
        );
    }

    public static IEnumerable<CreateAccountCommand> InvalidTestCases()
    {
        yield return new CreateAccountCommand(
            0,
            "name"
        );
        yield return new CreateAccountCommand(
            -1,
            ""
        );
        yield return new CreateAccountCommand(
            -234234,
            ""
        );
    }
}