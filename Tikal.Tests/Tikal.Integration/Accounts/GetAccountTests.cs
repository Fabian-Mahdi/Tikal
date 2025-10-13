using System.Net;
using System.Net.Http.Json;
using Tikal.Integration.Accounts.Data;
using Tikal.Integration.Core;
using Tikal.Presentation.Accounts.Dtos;

namespace Tikal.Integration.Accounts;

public class GetAccountTests : IntegrationTestFixture
{
    // constants
    private const string uri = "accounts";

    [Test]
    public async Task Given_User_Has_No_Account_When_Get_Account_Then_Returns_NotFound()
    {
        // when
        HttpResponseMessage response = await Client.GetAsync(uri);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [TestCaseSource(typeof(CreateAccountDtoSource), nameof(CreateAccountDtoSource.ValidTestCases))]
    public async Task Given_Existing_Account_For_User_When_Get_Account_Then_Returns_Ok(CreateAccountDto dto)
    {
        // given
        await Client.PostAsJsonAsync(uri, dto);

        // when
        HttpResponseMessage response = await Client.GetAsync(uri);

        // then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}