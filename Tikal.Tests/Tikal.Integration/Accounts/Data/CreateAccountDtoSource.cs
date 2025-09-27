using Tikal.Presentation.Accounts.Dtos;

namespace Tikal.Integration.Accounts.Data;

public static class CreateAccountDtoSource
{
    public static IEnumerable<CreateAccountDto> ValidTestCases()
    {
        yield return new CreateAccountDto("Account Name");
        yield return new CreateAccountDto("A much loooooooonger account name");
        yield return new CreateAccountDto(")§(/$)§/$=$§/()$");
    }

    public static IEnumerable<CreateAccountDto> InvalidTestCases()
    {
        yield return new CreateAccountDto("");
    }
}