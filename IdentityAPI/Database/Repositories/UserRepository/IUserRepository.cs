using IdentityAPI.Models;

namespace IdentityAPI.Database.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User> GetUser(string username, string password);
}