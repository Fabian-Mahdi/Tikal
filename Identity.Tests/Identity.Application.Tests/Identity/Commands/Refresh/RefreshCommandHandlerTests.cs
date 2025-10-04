using Identity.Application.Identity.Commands.Refresh;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using Moq;
using OneOf;

namespace Identity.Application.Tests.Identity.Commands.Refresh;

public class RefreshCommandHandlerTests
{
    // dummies
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    // dependencies
    private Mock<UserRepository> userRepository;

    private Mock<TokenRepository> tokenRepository;

    // under test
    private RefreshCommandHandler commandHandler;

    [SetUp]
    public void Setup()
    {
        userRepository = new Mock<UserRepository>();
        tokenRepository = new Mock<TokenRepository>();

        commandHandler = new RefreshCommandHandler(userRepository.Object, tokenRepository.Object);
    }

    private void SetupMocks(RefreshCommand command)
    {
        const string username = "username";

        User user = new(1, username);

        tokenRepository
            .Setup(r => r.ValidateToken(command.token))
            .ReturnsAsync(true);

        tokenRepository
            .Setup(r => r.ExtractClaim<string>(command.token, "name"))
            .ReturnsAsync(username);

        userRepository
            .Setup(r => r.FindByUsername(username, cancellationToken))
            .ReturnsAsync(user);

        TokenPair tokenPair = new("accessToken", "refreshToken");

        tokenRepository
            .Setup(r => r.GenerateTokenPair(user))
            .Returns(tokenPair);
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_ValidateToken(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        tokenRepository.Verify(r => r.ValidateToken(command.token), Times.Once);
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_ExtractClaim(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        tokenRepository.Verify(r => r.ExtractClaim<string>(command.token, "name"), Times.Once);
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_FindByUsername(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        userRepository.Verify(r => r.FindByUsername(It.IsAny<string>(), cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_GenerateTokenPair(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        tokenRepository.Verify(r => r.GenerateTokenPair(It.IsAny<User>()), Times.Once);
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Invalid_Token_When_Handle_Then_Returns_InvalidToken(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        tokenRepository
            .Setup(r => r.ValidateToken(command.token))
            .ReturnsAsync(false);

        // when
        OneOf<TokenPair, InvalidToken> result = await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<InvalidToken>());
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_NonExistent_Name_Claim_When_Handle_Then_Returns_InvalidToken(RefreshCommand command)
    {
        // given
        SetupMocks(command);

        tokenRepository
            .Setup(r => r.ExtractClaim<string>(command.token, "name"))
            .ReturnsAsync((string)null!);

        // when
        OneOf<TokenPair, InvalidToken> result = await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<InvalidToken>());
    }

    [TestCaseSource(
        typeof(RefreshCommandTestCasesSource),
        nameof(RefreshCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_No_User_With_Username_Equal_To_Name_Claim_When_Handle_Then_Returns_InvalidToken(
        RefreshCommand command
    )
    {
        // given
        SetupMocks(command);

        userRepository
            .Setup(r => r.FindByUsername(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync((User)null!);

        // when
        OneOf<TokenPair, InvalidToken> result = await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<InvalidToken>());
    }
}