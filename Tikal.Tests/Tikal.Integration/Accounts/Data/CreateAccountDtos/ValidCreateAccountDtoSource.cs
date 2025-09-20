using Tikal.Presentation.Accounts.Dtos;

namespace Tikal.Integration.Accounts.Data.CreateAccountDtos;

public static class ValidCreateAccountDtoSource
{
    public static IEnumerable<CreateAccountDto> TestCases()
    {
        yield return new CreateAccountDto("Account Name");
        yield return new CreateAccountDto("A much loooooooonger account name");
        yield return new CreateAccountDto(")§(/$)§/$=$§/()$");
    }
}