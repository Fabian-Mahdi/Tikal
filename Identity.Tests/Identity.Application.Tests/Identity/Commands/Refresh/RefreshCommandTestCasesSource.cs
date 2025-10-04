using Identity.Application.Identity.Commands.Refresh;

namespace Identity.Application.Tests.Identity.Commands.Refresh;

public static class RefreshCommandTestCasesSource
{
    public static IEnumerable<RefreshCommand> ValidTestCases()
    {
        yield return new RefreshCommand("");
        yield return new RefreshCommand("token");
        yield return new RefreshCommand("sldkfjswoelifsloaiejfklasjfeas");
    }
}