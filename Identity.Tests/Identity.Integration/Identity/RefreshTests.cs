using System.Net;
using System.Net.Http.Json;
using Identity.Integration.Core;
using Identity.Integration.Identity.Data;
using Identity.Presentation.Identity.Dtos;

namespace Identity.Integration.Identity;

public class RefreshTests : IntegrationTestFixture
{
    // constants
    private const string registerUri = "register";

    private const string loginUri = "login";

    private const string refreshUri = "refresh";

    private async Task<string> RegisterUserAndGetRefreshToken(RegisterDto registerDto)
    {
        await Client.PostAsJsonAsync(registerUri, registerDto);

        LoginDto loginDto = new(registerDto.username, registerDto.password);

        HttpResponseMessage response = await Client.PostAsJsonAsync(loginUri, loginDto);

        IEnumerable<string> cookies = response.Headers.GetValues("Set-Cookie");

        foreach (string cookie in cookies)
        {
            string[] parts = cookie.Split('=');

            if (parts[0] == "refresh_token")
            {
                return parts[1];
            }
        }

        return string.Empty;
    }

    [Test]
    public async Task Given_No_RefreshToken_When_Refresh_Then_Returns_Unauthorized()
    {
        // when
        HttpResponseMessage response = await Client.PostAsync(refreshUri, null);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TestCaseSource(
        typeof(RegisterDtoSource),
        nameof(RegisterDtoSource.ValidTestCases)
    )]
    public async Task Given_Valid_RefreshToken_When_Refresh_Then_Returns_Success(RegisterDto registerDto)
    {
        // given
        string refreshToken = await RegisterUserAndGetRefreshToken(registerDto);

        Client.DefaultRequestHeaders.Add("Cookie", $"refresh_token={refreshToken}");

        // when
        HttpResponseMessage response = await Client.PostAsync(refreshUri, null);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Given_Invalid_RefreshToken_When_Refresh_Then_Returns_Unauthorized()
    {
        // given
        const string refreshToken = "invalid_token";

        Client.DefaultRequestHeaders.Add("Cookie", $"refresh_token={refreshToken}");

        // when
        HttpResponseMessage response = await Client.PostAsync(refreshUri, null);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}