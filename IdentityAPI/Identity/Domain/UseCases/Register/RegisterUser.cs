using FluentResults;
using FluentValidation;
using IdentityAPI.Core.Exceptions;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.UseCases.Register;

/// <summary>
///     The use case for performing a user registration
/// </summary>
public class RegisterUser
{
    private readonly UserDataAccess userDataAccess;

    private readonly IValidator<User> userValidator;

    private readonly IValidator<string> passwordValidator;

    public RegisterUser(UserDataAccess userDataAccess, IValidator<User> userValidator,
        IValidator<string> passwordValidator)
    {
        this.userDataAccess = userDataAccess;
        this.userValidator = userValidator;
        this.passwordValidator = passwordValidator;
    }

    /// <summary>
    ///     Registers a new user with a given password
    /// </summary>
    /// <param name="user">The user to register</param>
    /// <param name="password">The password to assign</param>
    /// <returns>Void in case of success otherwise <see cref="RegisterError" /></returns>
    /// <remarks>Grants the User <see cref="Role" /> to the created user</remarks>
    public async Task<Result> Register(User user, string password)
    {
        if (!(await userValidator.ValidateAsync(user)).IsValid)
        {
            return Result.Fail(new InvalidUsername(user.Username));
        }

        if (!(await passwordValidator.ValidateAsync(password)).IsValid)
        {
            return Result.Fail(new InvalidPassword());
        }

        user.Roles.Add(Role.User);

        Result result = await userDataAccess.CreateUser(user, password);

        if (result.IsFailed)
        {
            return handleUserCreationError(result);
        }

        return Result.Ok();
    }

    /// <summary>
    ///     Handles user creation failure
    /// </summary>
    /// <param name="result">The failed result of a user creation operation</param>
    /// <returns>The appropriate instance of <see cref="RegisterError" /></returns>
    private Result handleUserCreationError(Result result)
    {
        IError? error = result.Errors.OfType<UserError>().FirstOrDefault();

        switch (error)
        {
            case UsernameUniqueConstraint usernameUniqueConstraint:
                return Result.Fail(new DuplicateUserName(usernameUniqueConstraint.Username));
            default:
                throw new UnknownErrorException();
        }
    }
}