using IdentityAPI.Authentication.Domain.Models;

namespace IdentityAPI.Authentication.Domain.DataAccess;

public interface TokenDataAccess
{
    (RefreshToken, AccessToken) GenerateTokenPair(User user);
}