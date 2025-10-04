using Identity.Presentation.Identity.Dtos;

namespace Identity.Integration.Identity.Data;

public static class LoginDtoSource
{
    public static IEnumerable<LoginDto> ValidTestCases()
    {
        yield return new LoginDto("username", "Password1!");
        yield return new LoginDto("f32ofskl3234", "sdlkfja32FSD§$");
    }

    public static IEnumerable<LoginDto> InvalidTestCases()
    {
        yield return new LoginDto("username", "");
        yield return new LoginDto("", "password");
    }
}