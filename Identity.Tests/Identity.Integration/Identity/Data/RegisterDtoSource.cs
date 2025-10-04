using Identity.Presentation.Identity.Dtos;

namespace Identity.Integration.Identity.Data;

public static class RegisterDtoSource
{
    public static IEnumerable<RegisterDto> ValidTestCases()
    {
        yield return new RegisterDto("username", "Password1!");
        yield return new RegisterDto("asdfnowier", "sldkfj234FD$%()");
    }

    public static IEnumerable<RegisterDto> InvalidTestCases()
    {
        yield return new RegisterDto("", "Password1!");
        yield return new RegisterDto("username", "");
        yield return new RegisterDto("username", "invalidPassword");
        yield return new RegisterDto("us", "Password1!");
    }
}