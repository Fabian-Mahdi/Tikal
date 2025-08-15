using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Tests.Data.Models;

public static class InvalidUserSources
{
    public static IEnumerable<User> TestCases()
    {
        yield return new User(
            "",
            ""
        );
        yield return new User(
            "toooooooooooooooooooooooooooooooooooooo long",
            "id"
        );
        yield return new User(
            "asd",
            "id"
        );
    }
}