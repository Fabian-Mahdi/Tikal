using Identity.Application.Core.Errors;
using Identity.Application.Identity.Commands.Login;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using Moq;
using OneOf;

namespace Identity.Application.Tests.Identity.Commands.Login;

public class LoginCommandHandlerTests
{
    // dummies
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    // dependencies
    private Mock<UserRepository> userRepository;

    private Mock<TokenRepository> tokenRepository;

    // under test
    private LoginCommandHandler commandHandler;

    [SetUp]
    public void SetUp()
    {
        userRepository = new Mock<UserRepository>();
        tokenRepository = new Mock<TokenRepository>();

        commandHandler = new LoginCommandHandler(userRepository.Object, tokenRepository.Object);
    }

    private void SetupMocks(LoginCommand command)
    {
        User user = new(command.username);

        userRepository
            .Setup(r => r.FindByUsername(command.username, cancellationToken))
            .ReturnsAsync(user);

        userRepository
            .Setup(r => r.ValidatePassword(user, command.password, cancellationToken))
            .ReturnsAsync(true);

        TokenPair tokenPair = new("accessToken", "refreshToken");

        tokenRepository
            .Setup(r => r.GenerateTokenPair(user))
            .Returns(tokenPair);
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_FindByUsername(LoginCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        userRepository.Verify(r => r.FindByUsername(command.username, cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_ValidatePassword(LoginCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        userRepository.Verify(
            r => r.ValidatePassword(It.IsAny<User>(), command.password, cancellationToken),
            Times.Once
        );
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_GenerateTokenPair(LoginCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        tokenRepository.Verify(r => r.GenerateTokenPair(It.IsAny<User>()), Times.Once);
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_No_User_With_Username_When_Handle_Then_Returns_InvalidCredentials(LoginCommand command)
    {
        // given
        SetupMocks(command);

        userRepository
            .Setup(r => r.FindByUsername(command.username, cancellationToken))
            .ReturnsAsync((User)null!);

        // when
        OneOf<TokenPair, ValidationFailed, InvalidCredentials> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<InvalidCredentials>());
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Invalid_Password_When_Handle_Then_Returns_InvalidCredentials(LoginCommand command)
    {
        // given
        SetupMocks(command);

        userRepository
            .Setup(r => r.ValidatePassword(It.IsAny<User>(), command.password, cancellationToken))
            .ReturnsAsync(false);

        // when
        OneOf<TokenPair, ValidationFailed, InvalidCredentials> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<InvalidCredentials>());
    }
}