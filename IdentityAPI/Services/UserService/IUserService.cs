using IdentityAPI.Models;

namespace IdentityAPI.Services.UserService;

public interface IUserService
{
    Task<User> CreateUser(User user);

    Task<User> GetUser(string username, string password);
}