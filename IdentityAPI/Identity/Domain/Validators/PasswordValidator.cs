using FluentValidation;

namespace IdentityAPI.Identity.Domain.Validators;

/// <summary>
///     Validates user passwords
/// </summary>
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).MinimumLength(8);
    }
}