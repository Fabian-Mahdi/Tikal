using IdentityAPI.Models;

namespace IdentityAPI.Services.TokenService;

public interface ITokenService
{
    TokenPair GenerateTokenPair(User user);
}