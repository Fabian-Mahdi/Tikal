using Tikal.Application.Accounts.Queries.GetAccount;

namespace Tikal.Application.Tests.Accounts.Queries.GetAccount;

public static class GetAccountQueryTestCasesSource
{
    public static IEnumerable<GetAccountQuery> ValidTestCases()
    {
        yield return new GetAccountQuery(0);
        yield return new GetAccountQuery(1);
        yield return new GetAccountQuery(2903482);
    }
}