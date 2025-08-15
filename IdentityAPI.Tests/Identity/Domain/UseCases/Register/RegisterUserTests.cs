using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Register;
using IdentityAPI.Tests.Data.Models;
using Moq;

namespace IdentityAPI.Tests.Identity.Domain.UseCases.Register;

public class RegisterUserTests
{
    // constants
    private const string password = "password";

    // dependencies
    private Mock<UserDataAccess> userDataAccess;

    private Mock<IValidator<User>> userValidator;

    private Mock<IValidator<string>> passwordValidator;

    // under test
    private RegisterUser registerUser;

    [SetUp]
    public void Setup()
    {
        userDataAccess = new Mock<UserDataAccess>();
        userValidator = new Mock<IValidator<User>>();
        passwordValidator = new Mock<IValidator<string>>();

        registerUser = new RegisterUser(userDataAccess.Object, userValidator.Object, passwordValidator.Object);
    }

    private void SetupMocks(User user)
    {
        userValidator
            .Setup(v => v.ValidateAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        passwordValidator
            .Setup(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        userDataAccess
            .Setup(u => u.CreateUser(user, password))
            .ReturnsAsync(Result.Ok());
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_Register_Then_Calls_UserValidator_Validate(User user)
    {
        // given
        SetupMocks(user);

        // when
        await registerUser.Register(user, password);

        // then
        userValidator.Verify(v => v.ValidateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_Invalid_User_When_Register_Then_Returns_InvalidUsername(User user)
    {
        // given
        SetupMocks(user);

        userValidator
            .Setup(v => v.ValidateAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("username", "test failure")]));

        // when
        Result result = await registerUser.Register(user, password);

        // then
        Assert.That(result.HasError<InvalidUsername>());
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_Register_Then_Calls_PasswordValidator_Validate(User user)
    {
        // given
        SetupMocks(user);

        // when
        await registerUser.Register(user, password);

        // then
        passwordValidator.Verify(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_Invalid_Password_When_Register_Then_Returns_InvalidPassword(User user)
    {
        // given
        SetupMocks(user);

        passwordValidator
            .Setup(v => v.ValidateAsync(password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("password", "test failure")]));

        // when
        Result result = await registerUser.Register(user, password);

        // then
        Assert.That(result.HasError<InvalidPassword>());
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_Register_Then_Calls_UserDataAccess_CreateUser(User user)
    {
        // given
        SetupMocks(user);

        // when
        await registerUser.Register(user, password);

        // then
        userDataAccess.Verify(u => u.CreateUser(user, password), Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_UsernameUniqueConstraint_Error_When_CreateUser_Then_Returns_DuplicateUsername(User user)
    {
        // given
        SetupMocks(user);

        userDataAccess
            .Setup(u => u.CreateUser(user, password))
            .ReturnsAsync(Result.Fail(new UsernameUniqueConstraint(user.Username)));

        // when
        Result result = await registerUser.Register(user, password);

        // then
        Assert.That(result.HasError<DuplicateUserName>());
    }
}