namespace IdentityAPI.Services.TokenService;

public interface ITokenService
{
    TokenPair GenerateTokenPair(Guid userId, string username);
}