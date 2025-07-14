using IdentityAPI.Models;

namespace IdentityAPI.Tests.Data.Models;

internal static class UserSource
{
    public static IEnumerable<User> TestCases()
    {
        yield return new User
        {
            Id = "id",
            UserName = "username"
        };
        yield return new User
        {
            Id = "",
            UserName = ""
        };
        yield return new User
        {
            Id =
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam",
            UserName =
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam"
        };
    }
}