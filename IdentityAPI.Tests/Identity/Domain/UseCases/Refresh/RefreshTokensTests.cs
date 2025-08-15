using System.IdentityModel.Tokens.Jwt;
using FluentResults;
using IdentityAPI.Identity.Domain.DataAccess.Tokens;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Refresh;
using Moq;

namespace IdentityAPI.Tests.Identity.Domain.UseCases.Refresh;

public class RefreshTokensTests
{
    // constants
    private const string username = "username";

    private const string token = "token";

    // dependencies
    private Mock<UserDataAccess> userDataAccess;

    private Mock<TokenDataAccess> tokenDataAccess;

    // under test
    private RefreshTokens refreshTokens;

    [SetUp]
    public void Setup()
    {
        userDataAccess = new Mock<UserDataAccess>();
        tokenDataAccess = new Mock<TokenDataAccess>();

        refreshTokens = new RefreshTokens(userDataAccess.Object, tokenDataAccess.Object);
    }

    private void SetupMocks()
    {
        TokenPair tokenPair = new("accessToken", "refreshToken");

        User user = new(username);

        tokenDataAccess
            .Setup(t => t.ValidateToken(token))
            .ReturnsAsync(true);

        tokenDataAccess
            .Setup(t => t.ExtractClaim<string>(token, JwtRegisteredClaimNames.Name))
            .ReturnsAsync(username);

        userDataAccess
            .Setup(u => u.FindByName(username))
            .ReturnsAsync(user);

        tokenDataAccess
            .Setup(t => t.GenerateTokenPair(user))
            .Returns(tokenPair);
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_ValidateToken()
    {
        // given
        SetupMocks();

        // when
        await refreshTokens.Refresh(token);

        // then
        tokenDataAccess.Verify(t => t.ValidateToken(token), Times.Once);
    }

    [Test]
    public async Task Given_Invalid_Token_When_Refresh_Then_Returns_InvalidToken()
    {
        // given
        SetupMocks();

        tokenDataAccess
            .Setup(t => t.ValidateToken(token))
            .ReturnsAsync(false);

        // when
        Result<TokenPair> result = await refreshTokens.Refresh(token);

        // then
        Assert.That(result.HasError<InvalidToken>());
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_ExtractClaim()
    {
        // given
        SetupMocks();

        // when
        await refreshTokens.Refresh(token);

        // then
        tokenDataAccess.Verify(t => t.ExtractClaim<string>(token, JwtRegisteredClaimNames.Name), Times.Once);
    }

    [Test]
    public async Task Given_NonExistent_Username_Claim_When_Refresh_Then_Returns_InvalidToken()
    {
        // given
        SetupMocks();

        tokenDataAccess
            .Setup(t => t.ExtractClaim<string>(token, JwtRegisteredClaimNames.Name))
            .ReturnsAsync((string)null!);

        // when
        Result<TokenPair> result = await refreshTokens.Refresh(token);

        // then
        Assert.That(result.HasError<InvalidToken>());
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_UserDataAccess_FindByName()
    {
        // given
        SetupMocks();

        // when
        await refreshTokens.Refresh(token);

        // then
        userDataAccess.Verify(t => t.FindByName(username), Times.Once);
    }

    [Test]
    public async Task Given_NonExistent_User_With_Given_Name_When_Refresh_Then_Returns_InvalidToken()
    {
        // given
        SetupMocks();

        userDataAccess
            .Setup(t => t.FindByName(username))
            .ReturnsAsync((User)null!);

        // when
        Result<TokenPair> result = await refreshTokens.Refresh(token);

        // then
        Assert.That(result.HasError<InvalidToken>());
    }

    [Test]
    public async Task Given_Token_When_Refresh_Then_Calls_TokenDataAccess_GenerateTokenPair()
    {
        // given
        SetupMocks();

        // when
        await refreshTokens.Refresh(token);

        // then
        tokenDataAccess.Verify(t => t.GenerateTokenPair(It.IsAny<User>()));
    }
}