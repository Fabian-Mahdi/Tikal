using IdentityAPI.Integration.Base;

namespace IdentityAPI.Integration.Endpoints;

public class RegisterApiTests : TestContainerFixture
{
    private CustomWebApplicationFactory factory;

    private HttpClient client;

    [SetUp]
    public void SetUp()
    {
        factory = new(DatabaseContainer.GetConnectionString());
        client = factory.CreateClient();
    }

    [Test]
    public async Task Test()
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/register", new { username = "username", password = "Password1!" });

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [TearDown]
    public void TearDown()
    {
        client.Dispose();
        factory.Dispose();
    }
}