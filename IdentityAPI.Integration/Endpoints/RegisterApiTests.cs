using IdentityAPI.Dtos;
using IdentityAPI.Integration.Base;
using IdentityAPI.Integration.Data.Dtos.Register;
using System.Net;

namespace IdentityAPI.Integration.Endpoints;

public class RegisterApiTests : TestContainerFixture
{
    private CustomWebApplicationFactory factory;

    private HttpClient client;

    // constants
    private const string uri = "/register";

    [SetUp]
    public void SetUp()
    {
        factory = new(DatabaseContainer.GetConnectionString());
        client = factory.CreateClient();
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Valid_RegisterDto_When_Register_Then_Returns_Success(RegisterDto registerDto)
    {
        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [TestCaseSource(typeof(InvalidRegisterDtoSource), nameof(InvalidRegisterDtoSource.TestCases))]
    public async Task Given_Invalid_RegisterDto_When_Register_Then_Returns_400(RegisterDto registerDto)
    {
        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Existing_Username_When_Register_Then_Returns_Error(RegisterDto registerDto)
    {
        // given
        await client.PostAsJsonAsync(uri, registerDto);

        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.IsSuccessStatusCode, Is.False);
    }

    [TearDown]
    public void TearDown()
    {
        client.Dispose();
        factory.Dispose();
    }
}