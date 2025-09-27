using Tikal.Application.Accounts.Commands.CreateAccount;

namespace Tikal.Application.Tests.Accounts.Commands.CreateAccount;

public static class CreateAccountCommandTestCasesSource
{
    public static IEnumerable<CreateAccountCommand> ValidTestCases()
    {
        yield return new CreateAccountCommand(
            "id",
            "name"
        );
        yield return new CreateAccountCommand(
            "20394820934",
            "20348290348"
        );
        yield return new CreateAccountCommand(
            "asdfajsdkflasödfj",
            "ajskdlföaieaslldf"
        );
    }

    public static IEnumerable<CreateAccountCommand> InvalidTestCases()
    {
        yield return new CreateAccountCommand(
            "",
            "name"
        );
        yield return new CreateAccountCommand(
            "id",
            ""
        );
        yield return new CreateAccountCommand(
            "",
            ""
        );
    }
}