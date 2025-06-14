using IdentityAPI.Database.Repositories.UserRepository.Exceptions;
using IdentityAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Database.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext context;

    public UserRepository(IdentityDbContext context)
    {
        this.context = context;
    }

    public async Task<User> GetUser(string username, string password)
    {
        return await context.Users.FirstOrDefaultAsync(user => user.Username == username && user.Password == password)
            ?? throw new UserNotFoundException();
    }
}