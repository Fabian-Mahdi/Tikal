using Testcontainers.PostgreSql;

namespace Identity.Integration.Core;

public class TestContainerFixture
{
    protected PostgreSqlContainer DatabaseContainer { get; } = PostgresDatabase.Instance;
}