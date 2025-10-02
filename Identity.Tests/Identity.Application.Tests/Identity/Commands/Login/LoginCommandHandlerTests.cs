using Identity.Application.Identity.Commands.LoginCommand;
using Identity.Application.Identity.DataAccess;
using Moq;

namespace Identity.Application.Tests.Identity.Commands.LoginCommand;

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
}