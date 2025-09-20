using Tikal.Application.Accounts.Commands.CreateAccount;

namespace Tikal.Application.Tests.Accounts.Commands.CreateAccount;

public static class CreateAccountCommandTestCasesSource
{
    public static IEnumerable<CreateAccountCommand> TestCases()
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
}