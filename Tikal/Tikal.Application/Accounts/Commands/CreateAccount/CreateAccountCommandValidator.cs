using FluentValidation;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

/// <summary>
///     The validator for <see cref="CreateAccountCommand" />
/// </summary>
public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.id).NotEmpty();
        RuleFor(c => c.name).NotEmpty();
    }
}