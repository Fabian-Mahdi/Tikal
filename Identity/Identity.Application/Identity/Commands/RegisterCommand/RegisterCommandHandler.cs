using Identity.Application.Core.Errors;
using Identity.Application.Core.Messaging;
using Identity.Application.Identity.DataAccess;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.RegisterCommand;

/// <summary>
///     The command handler for <see cref="RegisterCommand" />
/// </summary>
public class RegisterCommandHandler
    : CommandHandler<RegisterCommand, OneOf<User, ValidationFailed, DuplicateUsername>>
{
    private readonly UserRepository userRepository;

    public RegisterCommandHandler(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<OneOf<User, ValidationFailed, DuplicateUsername>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        User? existingUser = await userRepository.FindByUsername(request.username, cancellationToken);

        if (existingUser is not null)
        {
            return new DuplicateUsername(existingUser.Username);
        }

        User user = new(request.username);

        OneOf<User, DuplicateUsername> result =
            await userRepository.CreateUser(user, request.password, cancellationToken);

        return result.Match<OneOf<User, ValidationFailed, DuplicateUsername>>(
            createdUser => createdUser,
            duplicateUsername => duplicateUsername
        );
    }
}