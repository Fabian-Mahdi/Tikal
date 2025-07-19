using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using Moq;

namespace IdentityAPI.Tests.Authentication.Domain.UseCases;

public class RefreshTokensTests
{
    // constants
    private const string defaultClaim = "claim";

    private const string testToken = "token";

    private readonly User defaultUser = new("username");

    // dependencies
    private Mock<TokenDataAccess> tokenDataAccess;

    private Mock<UserDataAccess> userDataAccess;

    // under test
    private RefreshTokens refreshTokens;

    [SetUp]
    public void Setup()
    {
        tokenDataAccess = new Mock<TokenDataAccess>();

        tokenDataAccess
            .Setup(t => t.ValidateToken(It.IsAny<string>()))
            .ReturnsAsync(true);

        tokenDataAccess
            .Setup(t => t.ExtractClaim<string>(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(defaultClaim);

        tokenDataAccess
            .Setup(t => t.GenerateTokenPair(It.IsAny<User>()));

        userDataAccess = new Mock<UserDataAccess>();

        userDataAccess
            .Setup(u => u.FindByName(It.IsAny<string>()))
            .ReturnsAsync(defaultUser);

        refreshTokens = new RefreshTokens(tokenDataAccess.Object, userDataAccess.Object);
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_ValidateToken()
    {
        // when
        await refreshTokens.Refresh(testToken);

        // then
        tokenDataAccess.Verify(t => t.ValidateToken(testToken), Times.Once);
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_ExtractClaim()
    {
        // when
        await refreshTokens.Refresh(testToken);

        // then
        tokenDataAccess.Verify(t => t.ExtractClaim<string>(testToken, It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_UserDataAccess_FindByName()
    {
        // when
        await refreshTokens.Refresh(testToken);

        // then
        userDataAccess.Verify(t => t.FindByName(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_GenerateTokenPair()
    {
        // when
        await refreshTokens.Refresh(testToken);

        // then
        tokenDataAccess.Verify(t => t.GenerateTokenPair(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public void Given_InvalidToken_When_Refresh_Then_Throws_InvalidTokenException()
    {
        // given
        tokenDataAccess
            .Setup(t => t.ValidateToken(testToken))
            .ReturnsAsync(false);

        // when & then
        Assert.ThrowsAsync<InvalidTokenException>(async () => await refreshTokens.Refresh(testToken));
    }

    [Test]
    public void Given_Non_Existing_Claim_When_Refresh_Then_Throws_InvalidTokenException()
    {
        // given
        tokenDataAccess
            .Setup(t => t.ExtractClaim<string>(testToken, It.IsAny<string>()));

        // when & then
        Assert.ThrowsAsync<InvalidTokenException>(async () => await refreshTokens.Refresh(testToken));
    }

    [Test]
    public void Given_Non_Existing_User_When_Refresh_Then_Throws_InvalidTokenException()
    {
        // given
        userDataAccess
            .Setup(u => u.FindByName(It.IsAny<string>()));

        // when & then
        Assert.ThrowsAsync<InvalidTokenException>(async () => await refreshTokens.Refresh(testToken));
    }
}