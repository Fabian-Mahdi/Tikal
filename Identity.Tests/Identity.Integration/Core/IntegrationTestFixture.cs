namespace Identity.Integration.Core;

public class IntegrationTestFixture : TestContainerFixture
{
    private CustomWebApplicationFactory factory;

    protected HttpClient Client { get; private set; }

    [SetUp]
    public void SetUp()
    {
        factory = new CustomWebApplicationFactory(DatabaseContainer.GetConnectionString());
        Client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        Client.Dispose();
        factory.Dispose();
    }
}