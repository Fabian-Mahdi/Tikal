using Testcontainers.PostgreSql;

namespace Tikal.Integration.Core;

public abstract class TestContainerFixture
{
    protected PostgreSqlContainer DatabaseContainer { get; } = PostgresDatabase.Instance;
}