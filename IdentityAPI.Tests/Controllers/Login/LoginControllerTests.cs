using IdentityAPI.Controllers.Login;
using IdentityAPI.Controllers.Login.Errors;
using IdentityAPI.Dtos;
using IdentityAPI.Models;
using IdentityAPI.Services.TokenService;
using IdentityAPI.Tests.Data.Dtos;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityAPI.Tests.Controllers.Login;

internal class LoginControllerTests
{
    private readonly TokenPair defaultTokenPair = new()
    {
        AccessToken = "accessToken",
        RefreshToken = "refreshToken"
    };

    // constants
    private readonly User defaultUser = new();

    private readonly SignInResult failedResult = SignInResult.Failed;

    private readonly SignInResult successfulResult = SignInResult.Success;

    // dependencies
    private Mock<ITokenService> tokenService;

    private Mock<UserManager<User>> userManager;

    private Mock<SignInManager<User>> signInManager;

    // under test
    private LoginController loginController;

    [SetUp]
    public void SetUp()
    {
        userManager = MockHelpers.MockUserManager<User>();

        userManager
            .Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(defaultUser);

        tokenService = new Mock<ITokenService>();

        tokenService
            .Setup(s => s.GenerateTokenPair(defaultUser))
            .Returns(defaultTokenPair);

        signInManager = MockHelpers.MockSignInManager<User>();

        signInManager
            .Setup(s => s.CheckPasswordSignInAsync(defaultUser, It.IsAny<string>(), false))
            .ReturnsAsync(successfulResult);

        loginController = new LoginController(userManager.Object, signInManager.Object, tokenService.Object);
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public async Task Given_LoginDto_When_Login_Then_Calls_UserManager_FindByNameAsync(LoginDto loginDto)
    {
        // when
        await loginController.Login(loginDto);

        // then
        userManager.Verify(m => m.FindByNameAsync(loginDto.Username));
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public void Given_No_User_With_Username_When_Login_Then_Throws_InvalidCredentialsException(LoginDto loginDto)
    {
        // given
        userManager
            .Setup(m => m.FindByNameAsync(loginDto.Username))
            .ReturnsAsync((User?)null);

        // when & then
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await loginController.Login(loginDto)
        );
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public async Task Given_LoginDto_When_Login_Then_Calls_SignInManager_CheckPasswordSigninAsync(LoginDto loginDto)
    {
        // when
        await loginController.Login(loginDto);

        // then
        signInManager.Verify(m => m.CheckPasswordSignInAsync(defaultUser, loginDto.Password, false));
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public void Given_PasswordValidation_Fails_When_Login_Then_Throws_InvalidCredentialsException(LoginDto loginDto)
    {
        // given
        signInManager
            .Setup(m => m.CheckPasswordSignInAsync(defaultUser, loginDto.Password, false))
            .ReturnsAsync(failedResult);

        // when & then
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await loginController.Login(loginDto)
        );
    }

    [TestCaseSource(typeof(LoginDtoSource), nameof(LoginDtoSource.TestCases))]
    public async Task Given_LoginDto_When_Login_Then_Calls_TokenService_GenerateTokenPair(LoginDto loginDto)
    {
        // when
        await loginController.Login(loginDto);

        // then
        tokenService.Verify(t => t.GenerateTokenPair(defaultUser));
    }
}