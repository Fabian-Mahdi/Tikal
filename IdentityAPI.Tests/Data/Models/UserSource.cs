using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Tests.Data.Models;

public static class UserSource
{
    public static IEnumerable<User> TestCases()
    {
        yield return new User(
            "id",
            "username",
            [
                new Role(RoleType.User)
            ]
        );
        yield return new User(
            "203849028340234820934820",
            "fdiaospivuhcjpwaeoifj",
            [
                new Role(RoleType.Admin)
            ]
        );
        yield return new User(
            "",
            "",
            []
        );
    }
}