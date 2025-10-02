using Identity.Application.Core.Errors;
using Identity.Application.Core.Messaging;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.LoginCommand;

/// <summary>
///     The command handler for <see cref="LoginCommand" />
/// </summary>
public class LoginCommandHandler
    : CommandHandler<LoginCommand, OneOf<TokenPair, ValidationFailed, InvalidCredentials>>
{
    private readonly UserRepository userRepository;

    private readonly TokenRepository tokenRepository;

    public LoginCommandHandler(UserRepository userRepository, TokenRepository tokenRepository)
    {
        this.userRepository = userRepository;
        this.tokenRepository = tokenRepository;
    }

    public async Task<OneOf<TokenPair, ValidationFailed, InvalidCredentials>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await userRepository.FindByUsername(request.username, cancellationToken);

        if (user is null)
        {
            return new InvalidCredentials();
        }

        bool validPassword = await userRepository.ValidatePassword(user, request.password, cancellationToken);

        if (!validPassword)
        {
            return new InvalidCredentials();
        }

        TokenPair tokenPair = tokenRepository.GenerateTokenPair(user);

        return tokenPair;
    }
}