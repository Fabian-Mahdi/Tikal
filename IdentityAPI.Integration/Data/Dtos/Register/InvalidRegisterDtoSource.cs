using IdentityAPI.Identity.Presentation.Dtos;

namespace IdentityAPI.Integration.Data.Dtos.Register;

public static class InvalidRegisterDtoSource
{
    public static IEnumerable<RegisterDto> TestCases()
    {
        yield return new RegisterDto
        {
            Username = "",
            Password = ""
        };
        yield return new RegisterDto
        {
            Username = "username",
            Password = "This password does not contain a number"
        };
        yield return new RegisterDto
        {
            Username = "username",
            Password = "short1!"
        };
    }
}