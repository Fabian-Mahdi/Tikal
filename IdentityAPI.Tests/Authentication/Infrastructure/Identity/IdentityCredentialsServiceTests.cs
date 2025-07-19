using IdentityAPI.Authentication.Infrastructure.Entities;
using IdentityAPI.Authentication.Infrastructure.Identity;
using IdentityAPI.Tests.Data.Other;
using IdentityAPI.Tests.MockHelpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityAPI.Tests.Authentication.Infrastructure.Identity;

public class IdentityCredentialsServiceTests
{
    // constants
    private readonly SignInResult successfulResult = SignInResult.Success;

    private readonly SignInResult failedResult = SignInResult.Failed;

    // dependencies
    private Mock<SignInManager<ApplicationUser>> signInManager;

    // under test
    private IdentityCredentialsService identityCredentialsService;

    [SetUp]
    public void Setup()
    {
        signInManager = IdentityMockHelpers.MockSignInManager<ApplicationUser>();

        signInManager
            .Setup(s => s.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                false,
                false)
            )
            .ReturnsAsync(successfulResult);

        identityCredentialsService = new IdentityCredentialsService(signInManager.Object);
    }


    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_ValidateCredentials_Then_Calls_SignInManager_PasswordSingInAsync(
        string username,
        string password
    )
    {
        // when
        await identityCredentialsService.ValidateCredentials(username, password);

        // then
        signInManager.Verify(s => s.PasswordSignInAsync(username, password, false, false), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Successful_Password_SignIn_When_ValidateCredentials_Then_Returns_True(
        string username,
        string password
    )
    {
        // when
        bool result = await identityCredentialsService.ValidateCredentials(username, password);

        // then
        Assert.That(result, Is.True);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Unsuccessful_Password_SignIn_When_ValidateCredentials_Then_Returns_False(
        string username,
        string password
    )
    {
        // given
        signInManager
            .Setup(s => s.PasswordSignInAsync(
                username,
                password,
                false,
                false)
            )
            .ReturnsAsync(failedResult);

        // when
        bool result = await identityCredentialsService.ValidateCredentials(username, password);

        // then
        Assert.That(result, Is.False);
    }
}