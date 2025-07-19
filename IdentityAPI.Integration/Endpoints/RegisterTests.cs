using System.Net;
using IdentityAPI.Controllers.Register.Dtos;
using IdentityAPI.Integration.Base;
using IdentityAPI.Integration.Data.Dtos.Register;

namespace IdentityAPI.Integration.Endpoints;

public class RegisterTests : TestContainerFixture
{
    // constants
    private const string uri = "/register";

    private HttpClient client;
    private CustomWebApplicationFactory factory;

    [SetUp]
    public void SetUp()
    {
        factory = new CustomWebApplicationFactory(DatabaseContainer.GetConnectionString());
        client = factory.CreateClient();
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Valid_RegisterDto_When_Register_Then_Returns_Success(RegisterDto registerDto)
    {
        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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