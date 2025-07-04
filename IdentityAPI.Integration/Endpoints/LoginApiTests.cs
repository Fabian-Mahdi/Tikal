using IdentityAPI.Dtos;
using IdentityAPI.Integration.Base;
using IdentityAPI.Integration.Data.Dtos.Login;
using IdentityAPI.Integration.Data.Dtos.Register;
using System.Net;

namespace IdentityAPI.Integration.Endpoints;

public class LoginApiTests : TestContainerFixture
{
    private CustomWebApplicationFactory factory;

    private HttpClient client;

    // constants
    private const string loginUri = "/login";
    private const string registerUri = "/register";

    [SetUp]
    public void SetUp()
    {
        factory = new(DatabaseContainer.GetConnectionString());
        client = factory.CreateClient();
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Existing_User_When_Login_With_Correct_Credentials_Then_Returns_Success(RegisterDto registerDto)
    {
        // given
        await client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new()
        {
            Username = registerDto.Username,
            Password = registerDto.Password,
        };

        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [TestCaseSource(typeof(ValidRegisterDtoSource), nameof(ValidRegisterDtoSource.TestCases))]
    public async Task Given_Existing_User_When_Login_With_Incorrect_Credentials_Then_Returns_Unauthorized(RegisterDto registerDto)
    {
        // given
        await client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new()
        {
            Username = registerDto.Username,
            Password = "wrong password",
        };

        // when
        HttpResponseMessage response = await client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public async Task Given_NonExisiting_User_When_Login_Then_Return_Unauthorized(LoginDto loginDto)
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