using Testcontainers.PostgreSql;

namespace Identity.Integration.Core;

public static class PostgresDatabase
{
    private const string imageTag = "postgres:17.5";

    private static readonly Lazy<PostgreSqlContainer> instance =
        new(() => new PostgreSqlBuilder()
            .WithImage(imageTag)
            .Build()
        );

    public static PostgreSqlContainer Instance => instance.Value;
}