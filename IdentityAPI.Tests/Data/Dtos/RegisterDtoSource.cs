using IdentityAPI.Dtos;

namespace IdentityAPI.Tests.Data.Dtos;

internal static class RegisterDtoSource
{
    public static IEnumerable<RegisterDto> TestCases()
    {
        yield return new RegisterDto()
        {
            Username = "username",
            Password = "password",
        };
        yield return new RegisterDto()
        {
            Username = "",
            Password = "",
        };
        yield return new RegisterDto()
        {
            Username = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
            Password = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
        };
    }
}