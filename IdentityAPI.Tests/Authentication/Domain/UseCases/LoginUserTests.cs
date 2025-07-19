using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Tests.Data.Other;
using Moq;

namespace IdentityAPI.Tests.Authentication.Domain.UseCases;

public class LoginUserTests
{
    // constants
    private readonly User defaultUser = new("username");

    // dependencies
    private Mock<UserDataAccess> userDataAccess;

    private Mock<TokenDataAccess> tokenDataAccess;

    private Mock<CredentialsDataAccess> credentialsDataAccess;

    // under test
    private LoginUser loginUser;

    [SetUp]
    public void Setup()
    {
        userDataAccess = new Mock<UserDataAccess>();

        userDataAccess
            .Setup(u => u.FindByName(It.IsAny<string>()))
            .ReturnsAsync(defaultUser);

        tokenDataAccess = new Mock<TokenDataAccess>();

        tokenDataAccess
            .Setup(t => t.GenerateTokenPair(It.IsAny<User>()));

        credentialsDataAccess = new Mock<CredentialsDataAccess>();

        credentialsDataAccess
            .Setup(c => c.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        loginUser = new LoginUser(userDataAccess.Object, tokenDataAccess.Object, credentialsDataAccess.Object);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_CredentialsDataAccess_ValidateCredentials(
        string username,
        string password
    )
    {
        // when
        await loginUser.Login(username, password);

        // then
        credentialsDataAccess.Verify(c => c.ValidateCredentials(username, password), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_UserDataAccess_FindByName(
        string username,
        string password
    )
    {
        // when
        await loginUser.Login(username, password);

        // then
        userDataAccess.Verify(u => u.FindByName(username), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_TokenDataAccess_GenerateTokenPair(
        string username,
        string password
    )
    {
        // when
        await loginUser.Login(username, password);

        // then
        tokenDataAccess.Verify(t => t.GenerateTokenPair(It.IsAny<User>()), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public void Given_Invalid_Credentials_When_Login_Then_Throws_InvalidCredentialException(
        string username,
        string password
    )
    {
        // given
        credentialsDataAccess
            .Setup(c => c.ValidateCredentials(username, password))
            .ReturnsAsync(false);

        // when & then
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await loginUser.Login(username, password));
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public void Given_UserNotFound_When_Login_Then_Throws_InvalidCredentialException(
        string username,
        string password
    )
    {
        // given
        userDataAccess
            .Setup(u => u.FindByName(username));

        // when & then
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await loginUser.Login(username, password));
    }
}