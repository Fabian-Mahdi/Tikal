using System.Net;
using IdentityAPI.Identity.Presentation.Dtos;
using IdentityAPI.Integration.Base;
using IdentityAPI.Integration.Data.Dtos.Login;
using IdentityAPI.Integration.Data.Dtos.Register;

namespace IdentityAPI.Integration.Endpoints;

public class LoginTests : TestContainerFixture
{
    // constants
    private const string loginUri = "/login";
    private const string registerUri = "/register";

    private HttpClient client;
    private CustomWebApplicationFactory factory;

    [SetUp]
    public void SetUp()
    {
        factory = new CustomWebApplicationFactory(DatabaseContainer.GetConnectionString());
        client = factory.CreateClient();
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Existing_User_When_Login_With_Correct_Credentials_Then_Returns_Success(
        RegisterDto registerDto
    )
    {
        // given
        await client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new()
        {
            Username = registerDto.Username,
            Password = registerDto.Password
        };

        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Existing_User_When_Login_With_Incorrect_Credentials_Then_Returns_Unauthorized(
        RegisterDto registerDto)
    {
        // given
        await client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new()
        {
            Username = registerDto.Username,
            Password = "wrong password"
        };

        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCaseSource(typeof(ValidLoginDtoSource), nameof(ValidLoginDtoSource.TestCases))]
    public async Task Given_NonExisting_User_When_Login_Then_Return_Unauthorized(LoginDto loginDto)
    {
        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TearDown]
    public void TearDown()
    {
        client.Dispose();
        factory.Dispose();
    }
}