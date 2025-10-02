using Identity.Application.Core.Messaging;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.Refresh;

/// <summary>
///     The command handler for <see cref="RefreshCommand" />
/// </summary>
public class RefreshCommandHandler
    : CommandHandler<RefreshCommand, OneOf<TokenPair, InvalidToken>>
{
    private readonly UserRepository userRepository;

    private readonly TokenRepository tokenRepository;

    public RefreshCommandHandler(UserRepository userRepository, TokenRepository tokenRepository)
    {
        this.userRepository = userRepository;
        this.tokenRepository = tokenRepository;
    }

    public async Task<OneOf<TokenPair, InvalidToken>> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken
    )
    {
        bool tokenIsValid = await tokenRepository.ValidateToken(request.token);

        if (!tokenIsValid)
        {
            return new InvalidToken(request.token);
        }

        string? username = await tokenRepository.ExtractClaim<string>(request.token, "name");

        if (username is null)
        {
            return new InvalidToken(request.token);
        }

        User? user = await userRepository.FindByUsername(username, cancellationToken);

        if (user is null)
        {
            return new InvalidToken(request.token);
        }

        TokenPair tokenPair = tokenRepository.GenerateTokenPair(user);

        return tokenPair;
    }
}