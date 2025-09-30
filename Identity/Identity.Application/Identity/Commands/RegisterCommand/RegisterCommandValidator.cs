using FluentValidation;

namespace Identity.Application.Identity.Commands.RegisterCommand;

/// <summary>
///     The validator for <see cref="RegisterCommand" />
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.username)
            .MinimumLength(4)
            .WithMessage("Username must be at least 4 characters long.")
            .MaximumLength(20)
            .WithMessage("Username must be at most 20 characters long.");

        RuleFor(x => x.password)
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special letter");
    }
}