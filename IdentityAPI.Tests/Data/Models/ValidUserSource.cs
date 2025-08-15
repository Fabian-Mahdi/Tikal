using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Tests.Data.Models;

public static class ValidUserSource
{
    public static IEnumerable<User> TestCases()
    {
        yield return new User(
            "username",
            "id"
        );
        yield return new User(
            "20938429034234",
            "23940234892034"
        );
        yield return new User(
            "with symbols!/§%/()=",
            "(%)=§/=)(§=$()"
        );
    }
}