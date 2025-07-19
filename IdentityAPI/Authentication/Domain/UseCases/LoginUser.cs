using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.UseCases;

public class LoginUser
{
    private readonly UserDataAccess userDataAccess;

    private readonly TokenDataAccess tokenDataAccess;

    private readonly CredentialsDataAccess credentialsDataAccess;

    public LoginUser(
        UserDataAccess userDataAccess,
        TokenDataAccess tokenDataAccess,
        CredentialsDataAccess credentialsDataAccess
    )
    {
        this.userDataAccess = userDataAccess;
        this.tokenDataAccess = tokenDataAccess;
        this.credentialsDataAccess = credentialsDataAccess;
    }

    public async Task<(RefreshToken, AccessToken)> Login(string username, string password)
    {
        bool validCredentials = await credentialsDataAccess.ValidateCredentials(username, password);

        if (!validCredentials)
        {
            throw new InvalidCredentialsException();
        }

        User? user = await userDataAccess.FindByName(username);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        return tokenDataAccess.GenerateTokenPair(user);
    }
}