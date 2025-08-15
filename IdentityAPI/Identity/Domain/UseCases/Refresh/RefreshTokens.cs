using System.IdentityModel.Tokens.Jwt;
using FluentResults;
using IdentityAPI.Identity.Domain.DataAccess.Tokens;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;

namespace IdentityAPI.Identity.Domain.UseCases.Refresh;

/// <summary>
///     The use case for refreshing a token pair
/// </summary>
public class RefreshTokens
{
    private readonly UserDataAccess userDataAccess;

    private readonly TokenDataAccess tokenDataAccess;

    public RefreshTokens(UserDataAccess userDataAccess, TokenDataAccess tokenDataAccess)
    {
        this.userDataAccess = userDataAccess;
        this.tokenDataAccess = tokenDataAccess;
    }

    /// <summary>
    ///     Generates a new <see cref="TokenPair" /> for the user that corresponds to the given token
    /// </summary>
    /// <param name="token">The token for which a new pair should be generated</param>
    /// <returns><see cref="TokenPair" /> in case of success, otherwise <see cref="RefreshError" /></returns>
    public async Task<Result<TokenPair>> Refresh(string token)
    {
        bool tokenIsValid = await tokenDataAccess.ValidateToken(token);

        if (!tokenIsValid)
        {
            return Result.Fail(new InvalidToken());
        }

        string? username = await tokenDataAccess.ExtractClaim<string>(token, JwtRegisteredClaimNames.Name);

        if (username == null)
        {
            return Result.Fail(new InvalidToken());
        }

        User? user = await userDataAccess.FindByName(username);

        if (user == null)
        {
            return Result.Fail(new InvalidToken());
        }

        TokenPair tokenPair = tokenDataAccess.GenerateTokenPair(user);

        return Result.Ok(tokenPair);
    }
}