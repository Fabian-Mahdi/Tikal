using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Tests.Data.Models;

public static class UserSource
{
    public static IEnumerable<User> TestCases()
    {
        yield return new User(
            "id",
            "username"
        );
        yield return new User(
            "203849028340234820934820",
            "fdiaospivuhcjpwaeoifj"
        );
        yield return new User(
            "",
            ""
        );
    }
}