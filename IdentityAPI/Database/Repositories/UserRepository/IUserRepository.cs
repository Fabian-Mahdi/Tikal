using IdentityAPI.Models;

namespace IdentityAPI.Database.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User> CreateUser(User user);

    Task<User> GetUser(string username, string password);
}