using IdentityAPI.Database;
using IdentityAPI.Models;

namespace IdentityAPI.Services.UserService.Impl;

public class UserService : IUserService
{
    private readonly UnitOfWork unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUser(string username, string password)
    {
        return await unitOfWork.UserRepository.GetUser(username, password);
    }
}