using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Tests.Data.Models;
using Moq;

namespace IdentityAPI.Tests.Authentication.Domain.UseCases;

public class RegisterUserTests
{
    // constants
    private readonly UserCreationResult successfulResult = UserCreationResult.Success();

    private readonly UserCreationResult failedResult = UserCreationResult.Failed(["error"]);

    private readonly string testPassword = "password";

    // dependencies
    private Mock<UserDataAccess> userDataAccess;

    // under test
    private RegisterUser registerUser;

    [SetUp]
    public void Setup()
    {
        userDataAccess = new Mock<UserDataAccess>();

        userDataAccess
            .Setup(u => u.CreateUser(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(successfulResult);

        registerUser = new RegisterUser(userDataAccess.Object);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_Register_Then_Calls_userDataAccess_CreateUser(User user)
    {
        // when
        await registerUser.Register(user, testPassword);

        // then
        userDataAccess.Verify(u => u.CreateUser(user, testPassword), Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_SuccessfulCreation_When_Register_Then_Returns_Success_Result(User user)
    {
        // when
        RegisterResult result = await registerUser.Register(user, testPassword);

        // then
        Assert.That(result.Succeeded, Is.True);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_UnsuccessfulCreation_When_Register_Then_Returns_Failed_Result(User user)
    {
        // given
        userDataAccess
            .Setup(u => u.CreateUser(user, It.IsAny<string>()))
            .ReturnsAsync(failedResult);

        // when
        RegisterResult result = await registerUser.Register(user, testPassword);

        // then
        Assert.That(result.Succeeded, Is.False);
    }
}