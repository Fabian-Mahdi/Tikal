using FluentValidation;

namespace Identity.Application.Identity.Commands.LoginCommand;

/// <summary>
///     The validator for <see cref="LoginCommand" />
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.username)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}