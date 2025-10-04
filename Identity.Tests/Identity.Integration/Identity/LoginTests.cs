using System.Net;
using System.Net.Http.Json;
using Identity.Integration.Core;
using Identity.Integration.Identity.Data;
using Identity.Presentation.Identity.Dtos;

namespace Identity.Integration.Identity;

public class LoginTests : IntegrationTestFixture
{
    // constants
    private const string loginUri = "login";

    private const string registerUri = "register";

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.ValidTestCases))]
    public async Task Given_Existing_User_When_Login_With_Correct_Credentials_Then_Returns_Success(
        RegisterDto registerDto
    )
    {
        // given
        await Client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new(registerDto.username, registerDto.password);

        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.ValidTestCases))]
    public async Task Given_Existing_User_With_Incorrect_Credentials_When_Login_Then_Returns_Unauthorized(
        RegisterDto registerDto
    )
    {
        // given
        await Client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new(registerDto.username, "wrong password");

        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.ValidTestCases))]
    public async Task Given_NonExisting_User_When_Login_Then_Returns_Unauthorized(
        LoginDto loginDto
    )
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.InvalidTestCases))]
    public async Task Given_Invalid_LoginDto_When_Login_Then_Returns_BadRequest(
        LoginDto loginDto
    )
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(loginUri, loginDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}