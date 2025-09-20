using Testcontainers.PostgreSql;

namespace Tikal.Integration.Core;

public abstract class TestContainerFixture
{
    protected PostgreSqlContainer DatabaseContainer { get; private set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        DatabaseContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.5")
            .Build();

        await DatabaseContainer.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await DatabaseContainer.DisposeAsync();
    }
}