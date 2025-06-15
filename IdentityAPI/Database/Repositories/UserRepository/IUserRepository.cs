using IdentityAPI.Models;

namespace IdentityAPI.Database.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User> CreateUser(User user);

    Task<bool> GetUsernameAvailability(string username);

    Task<User> GetUser(string username, string password);
}