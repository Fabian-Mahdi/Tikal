namespace IdentityAPI.Authentication.Domain.DataAccess;

public interface CredentialsDataAccess
{
    Task<bool> ValidateCredentials(string username, string password);
}