using Identity.Application.Core.Errors;
using Identity.Application.Identity.Commands.Register;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using Moq;
using OneOf;

namespace Identity.Application.Tests.Identity.Commands.Register;

public class RegisterCommandHandlerTests
{
    // dummies
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    // dependencies
    private Mock<UserRepository> userRepository;

    // under test
    private RegisterCommandHandler commandHandler;

    [SetUp]
    public void Setup()
    {
        userRepository = new Mock<UserRepository>();

        commandHandler = new RegisterCommandHandler(userRepository.Object);
    }

    private void SetupMocks(RegisterCommand command)
    {
        userRepository
            .Setup(r => r.FindByUsername(command.username, cancellationToken))
            .ReturnsAsync((User)null!);

        User user = new(1, command.username);

        userRepository
            .Setup(r => r.CreateUser(It.IsAny<User>(), command.password, cancellationToken))
            .ReturnsAsync(user);
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_FindByUsername(RegisterCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        userRepository.Verify(r => r.FindByUsername(command.username, cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_CreateUser(RegisterCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        userRepository.Verify(r => r.CreateUser(It.IsAny<User>(), command.password, cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Existing_User_With_Username_When_Handle_Then_Returns_DuplicateUsername(
        RegisterCommand command
    )
    {
        // given
        SetupMocks(command);

        User existingUser = new(1, command.username);

        userRepository
            .Setup(r => r.FindByUsername(command.username, cancellationToken))
            .ReturnsAsync(existingUser);

        // when
        OneOf<User, ValidationFailed, DuplicateUsername> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<DuplicateUsername>());
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_User_Creation_Fails_When_Handle_Then_Returns_DuplicateUsername(
        RegisterCommand command
    )
    {
        // given
        SetupMocks(command);

        userRepository
            .Setup(r => r.CreateUser(It.IsAny<User>(), command.password, cancellationToken))
            .ReturnsAsync(new DuplicateUsername(command.username));

        // when
        OneOf<User, ValidationFailed, DuplicateUsername> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<DuplicateUsername>());
    }
}