using FluentValidation;

namespace Identity.Application.Identity.Commands.RegisterCommand;

/// <summary>
///     The validator for <see cref="RegisterCommand" />
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.username).NotEmpty();
        RuleFor(x => x.password).NotEmpty();
    }
}