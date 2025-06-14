using IdentityAPI.Models;

namespace IdentityAPI.Services.UserService;

public interface IUserService
{
    Task<User?> GetUser(string username, string password);
}