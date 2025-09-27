using System.Net;
using System.Net.Http.Json;
using Tikal.Integration.Accounts.Data;
using Tikal.Integration.Core;
using Tikal.Presentation.Accounts.Dtos;

namespace Tikal.Integration.Accounts;

public class CreateAccountTests : IntegrationTestFixture
{
    // constants
    private const string uri = "accounts";

    [TestCaseSource(typeof(CreateAccountDtoSource), nameof(CreateAccountDtoSource.ValidTestCases))]
    public async Task Given_User_Has_No_Account_When_Create_Account_Then_Creates_Account(CreateAccountDto dto)
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, dto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCaseSource(typeof(CreateAccountDtoSource), nameof(CreateAccountDtoSource.ValidTestCases))]
    public async Task Given_User_Has_Account_When_Create_Account_Then_Returns_Conflict(CreateAccountDto dto)
    {
        // given
        await Client.PostAsJsonAsync(uri, dto);

        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, dto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [TestCaseSource(typeof(CreateAccountDtoSource), nameof(CreateAccountDtoSource.InvalidTestCases))]
    public async Task Given_Invalid_Dto_When_Create_Account_Then_Returns_BadRequest(CreateAccountDto dto)
    {
        // when
        HttpResponseMessage response = await Client.PostAsJsonAsync(uri, dto);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}