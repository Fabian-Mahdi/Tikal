using FluentResults;
using FluentValidation;
using IdentityAPI.Identity.Domain.DataAccess.Tokens;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.UseCases.Login;

/// <summary>
///     The use case for performing a login
/// </summary>
public class LoginUser
{
    private readonly UserDataAccess userDataAccess;

    private readonly TokenDataAccess tokenDataAccess;

    private readonly IValidator<string> passwordValidator;

    public LoginUser(UserDataAccess userDataAccess, TokenDataAccess tokenDataAccess,
        IValidator<string> passwordValidator)
    {
        this.userDataAccess = userDataAccess;
        this.tokenDataAccess = tokenDataAccess;
        this.passwordValidator = passwordValidator;
    }

    /// <summary>
    ///     Retrieves a pair of tokens for the user corresponding to the given credentials
    /// </summary>
    /// <param name="username">The username of the user</param>
    /// <param name="password">The password of the user</param>
    /// <returns><see cref="TokenPair" /> for valid credentials, otherwise <see cref="LoginError" /></returns>
    public async Task<Result<TokenPair>> Login(string username, string password)
    {
        if (!(await passwordValidator.ValidateAsync(password)).IsValid)
        {
            return Result.Fail<TokenPair>(new InvalidCredentials());
        }

        User? user = await userDataAccess.FindByName(username);

        if (user == null)
        {
            return Result.Fail<TokenPair>(new InvalidCredentials());
        }

        bool validCredentials = await userDataAccess.ValidatePassword(user, password);

        if (!validCredentials)
        {
            return Result.Fail<TokenPair>(new InvalidCredentials());
        }

        TokenPair tokenPair = tokenDataAccess.GenerateTokenPair(user);

        return Result.Ok(tokenPair);
    }
}