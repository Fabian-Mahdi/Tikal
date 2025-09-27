using FluentValidation;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.id).NotEmpty();
        RuleFor(c => c.name).NotEmpty();
    }
}