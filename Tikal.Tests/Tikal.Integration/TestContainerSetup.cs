using Testcontainers.PostgreSql;
using Tikal.Integration.Core;

namespace Tikal.Integration;

[SetUpFixture]
public class TestContainerSetup
{
    private PostgreSqlContainer databaseContainer;

    [OneTimeSetUp]
    public async Task Setup()
    {
        databaseContainer = PostgresDatabase.Instance;

        await databaseContainer.StartAsync();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await databaseContainer.StopAsync();
    }
}