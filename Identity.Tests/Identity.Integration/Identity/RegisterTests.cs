using System.Net;
using System.Net.Http.Json;
using Identity.Integration.Core;
using Identity.Integration.Identity.Data;
using Identity.Presentation.Identity.Dtos;

namespace Identity.Integration.Identity;

public class RegisterTests : IntegrationTestFixture
{
    // constants
    private const string uri = "register";

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.ValidTestCases))]
    public async Task Given_Valid_RegisterDto_When_Register_Then_Returns_Success(RegisterDto registerDto)
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.InvalidTestCases))]
    public async Task Given_Invalid_RegisterDto_When_Register_Then_Returns_BadRequest(RegisterDto registerDto)
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.ValidTestCases))]
    public async Task Given_Existing_Username_When_Register_Then_Returns_Conflict(RegisterDto registerDto)
    {
        // given
        await Client.PostAsJsonAsync(uri, registerDto);

        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, registerDto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}