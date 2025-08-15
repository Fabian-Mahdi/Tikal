using FluentValidation;
using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.Validators;

/// <summary>
///     Validates all properties of a <see cref="User" />
/// </summary>
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username).MinimumLength(4).MaximumLength(20);
    }
}