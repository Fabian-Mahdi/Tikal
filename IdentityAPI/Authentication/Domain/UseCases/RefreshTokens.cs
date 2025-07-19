using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.UseCases;

public class RefreshTokens
{
    private readonly TokenDataAccess tokenDataAccess;

    private readonly UserDataAccess userDataAccess;

    public RefreshTokens(TokenDataAccess tokenDataAccess, UserDataAccess userDataAccess)
    {
        this.tokenDataAccess = tokenDataAccess;
        this.userDataAccess = userDataAccess;
    }

    public async Task<(RefreshToken, AccessToken)> Refresh(string token)
    {
        bool isValid = await tokenDataAccess.ValidateToken(token);

        if (!isValid)
        {
            throw new InvalidTokenException();
        }

        string? username = await tokenDataAccess.ExtractClaim<string>(token, "name");

        if (username == null)
        {
            throw new InvalidTokenException();
        }

        User? user = await userDataAccess.FindByName(username);

        if (user == null)
        {
            throw new InvalidTokenException();
        }

        return tokenDataAccess.GenerateTokenPair(user);
    }
}