using IdentityAPI.Controllers.Login.Dtos;

namespace IdentityAPI.Integration.Data.Dtos.Login;

public class ValidLoginDtoSource
{
    public static IEnumerable<LoginDto> TestCases()
    {
        yield return new LoginDto
        {
            Username = "username",
            Password = "password"
        };
        yield return new LoginDto
        {
            Username =
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
            Password =
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam"
        };
    }
}