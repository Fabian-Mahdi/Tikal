using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using IdentityAPI.Identity.Domain.DataAccess.Tokens;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Login;
using IdentityAPI.Tests.Data.Other;
using Moq;

namespace IdentityAPI.Tests.Identity.Domain.UseCases.Login;

public class LoginUserTests
{
    // dependencies
    private Mock<UserDataAccess> userDataAccess;

    private Mock<TokenDataAccess> tokenDataAccess;

    private Mock<IValidator<string>> passwordValidator;

    // under test
    private LoginUser loginUser;

    [SetUp]
    public void SetUp()
    {
        userDataAccess = new Mock<UserDataAccess>(MockBehavior.Strict);
        tokenDataAccess = new Mock<TokenDataAccess>(MockBehavior.Strict);
        passwordValidator = new Mock<IValidator<string>>(MockBehavior.Strict);

        loginUser = new LoginUser(userDataAccess.Object, tokenDataAccess.Object, passwordValidator.Object);
    }

    private void SetupMocks(string username, string password)
    {
        User user = new(username);

        TokenPair tokenPair = new("accessToken", "refreshToken");

        userDataAccess
            .Setup(u => u.FindByName(username))
            .ReturnsAsync(user);

        userDataAccess
            .Setup(u => u.ValidatePassword(user, password))
            .ReturnsAsync(true);

        tokenDataAccess
            .Setup(t => t.GenerateTokenPair(user))
            .Returns(tokenPair);

        passwordValidator
            .Setup(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_PasswordValidator_Validate(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        // when
        await loginUser.Login(username, password);

        // then
        passwordValidator.Verify(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Invalid_Password_When_Login_Then_Returns_Invalid_Credentials(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        passwordValidator
            .Setup(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("password", "testing failure")]));

        // when
        Result<TokenPair> result = await loginUser.Login(username, password);

        // then
        Assert.That(result.HasError<InvalidCredentials>());
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_UserDataAccess_FindByName(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        // when
        await loginUser.Login(username, password);

        // then
        userDataAccess.Verify(u => u.FindByName(username), Times.Once);
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_No_User_With_Given_Username_When_Login_Then_Returns_Invalid_Credentials(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        userDataAccess
            .Setup(u => u.FindByName(username))
            .ReturnsAsync((User?)null);

        // when
        Result<TokenPair> result = await loginUser.Login(username, password);

        // then
        Assert.That(result.HasError<InvalidCredentials>());
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_UserDataAccess_ValidatePassword(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        // when
        await loginUser.Login(username, password);

        // then
        userDataAccess.Verify(u => u.ValidatePassword(It.IsAny<User>(), password));
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Incorrect_Password_For_User_When_Login_Then_Returns_Invalid_Credentials(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        userDataAccess
            .Setup(u => u.ValidatePassword(It.IsAny<User>(), password))
            .ReturnsAsync(false);

        // when
        Result<TokenPair> result = await loginUser.Login(username, password);

        // then
        Assert.That(result.HasError<InvalidCredentials>());
    }

    [TestCaseSource(typeof(UsernamePasswordSource), nameof(UsernamePasswordSource.TestCases))]
    public async Task Given_Credentials_When_Login_Then_Calls_TokenDataAccess_GenerateTokenPair(
        string username,
        string password
    )
    {
        // given
        SetupMocks(username, password);

        // when
        await loginUser.Login(username, password);

        // then
        tokenDataAccess.Verify(t => t.GenerateTokenPair(It.IsAny<User>()));
    }
}