using IdentityAPI.Dtos;

namespace IdentityAPI.Integration.Data.Dtos.Register;

public static class ValidRegisterDtoSource
{
    public static IEnumerable<RegisterDto> TestCases()
    {
        yield return new RegisterDto()
        {
            Username = "username",
            Password = "Password1!",
        };
        yield return new RegisterDto()
        {
            Username = "scytale",
            Password = "GrIy4£647#8)",
        };
    }
}
